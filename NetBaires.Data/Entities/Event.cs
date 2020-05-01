#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using NetBaires.Data.DomainEvents;
using NetBaires.Events.DomainEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetBaires.Data.Entities
{
    public class Event : Entity
    {
        public string? Title { get; set; }
        public int? EmailTemplateThanksSponsorsId { get; set; }
        public Template? EmailTemplateThanksSponsors { get; set; }
        public int? EmailTemplateThanksSpeakersId { get; set; }
        public Template? EmailTemplateThanksSpeakers { get; set; }
        public int? EmailTemplateThanksAttendedId { get; set; }
        public Template? EmailTemplateThanksAttended { get; set; }
        public string? Description { get; set; } = String.Empty;
        [JsonConverter(typeof(StringEnumConverter))]
        public EventPlatform Platform { get; set; }
        public string? ImageUrl { get; set; } = String.Empty;
        public string? Url { get; set; } = String.Empty;
        public List<Attendance> Attendees { get; set; } = new List<Attendance>();
        public string? EventId { get; set; } = String.Empty;
        public decimal EstimatedAttendancePercentage { get; set; }
        public bool Done { get; protected set; } = false;
        public bool Live { get; set; } = false;
        public bool Online { get; set; } = false;
        public string? OnlineLink { get; set; } = String.Empty;
        public bool GeneralAttended { get; set; } = false;
        public string? GeneralAttendedCode { get; set; } = String.Empty;
        public DateTime? StartLiveTime { get; set; }
        public DateTime? EndLiveTime { get; set; }
        public DateTime Date { get; set; }
        public List<SponsorEvent> Sponsors { get; set; } = new List<SponsorEvent>();
        public List<GroupCode> GroupCodes { get; protected set; } = new List<GroupCode>();
        public List<Material> Materials { get; set; } = new List<Material>();
        public List<EventInformation> Information { get; set; } = new List<EventInformation>();

        public DomainResponse SetEmailTemplateThanksAttended(Template template)
        {
            if (template.Type != TemplateTypeEnum.EmailTemplateThanksAttended)
                return DomainResponse.Error();

            EmailTemplateThanksAttended = template;
            return DomainResponse.Ok();
        }

        public DomainResponse SetEmailTemplateThanksSpeakers(Template template)
        {
            if (template.Type != TemplateTypeEnum.EmailTemplateThanksSpeakers)
                return DomainResponse.Error();

            EmailTemplateThanksSpeakers = template;
            return DomainResponse.Ok();
        }

        public DomainResponse SetEmailTemplateThanksSponsors(Template template)
        {
            if (template.Type != TemplateTypeEnum.EmailTemplateThanksSponsors)
                return DomainResponse.Error();

            EmailTemplateThanksSponsors = template;
            return DomainResponse.Ok();
        }

        public DomainResponse AssignBadgeToAttended(Badge badge)
        {
            if (!Done)
                return DomainResponse.Error("El evento debe estar en Done antes de asignar Badges");

            foreach (var item in Attendees)
                if (item.Attended)
                {
                    badge.Members.Add(new BadgeMember
                    {
                        BadgeId = badge.Id,
                        MemberId = item.MemberId
                    });
                    AddDomainEvent(new AssignedBadgeToMember(item.MemberId, badge.Id));
                }
            return DomainResponse.Ok();
        }

        public void AddMaterial(string title, string link)
        {
            Materials.Add(new Material(title, link));
        }
        public void RemoveMaterial(Material material)
        {
            Materials.Remove(material);
        }


        public void AddInformation(string title, string description, bool visible)
        {
            Information.Add(new EventInformation(title, description, visible));
        }
        public void RemoveInformation(EventInformation information)
        {
            Information.Remove(information);
        }

        public DomainResponse AssignBadgeToAttended(Badge badge, Member member)
        {
            if (!Done)
                return DomainResponse.Error("El evento debe estar en Done antes de asignar Badges");
            var attended = Attendees.FirstOrDefault(a => a.MemberId == member.Id
                                              &&
                                              a.Attended);
            if (attended == null)
                return DomainResponse.Error("El miembro tiene que estar como que asistio al evento");

            badge.Members.Add(new BadgeMember
            {
                BadgeId = badge.Id,
                MemberId = member.Id
            });
            AddDomainEvent(new AssignedBadgeToMember(member.Id, badge.Id));
            return DomainResponse.Ok();
        }

        public void SetDone()
        {
            Done = true;
        }

        public Attendance AddAttendance(Member member, AttendanceRegisterType attendanceRegisterType)
        {
            var attendance = new Attendance(member, this, attendanceRegisterType);
            Attendees.Add(attendance);
            return attendance;
        }
        public Attendance Attended(Member member, AttendanceRegisterType attendanceRegisterType)
        {
            var memberCheck = Attendees.FirstOrDefault(x => x.MemberId == member.Id);
            if (memberCheck != null)
                memberCheck.Attend();
            else
            {
                memberCheck = AddAttendance(member, attendanceRegisterType);
                memberCheck.Attend();
            }
            return memberCheck;
        }
        public Attendance AddSpeaker(Member member)
        {
            var memberCheck = Attendees.FirstOrDefault(x => x.MemberId == member.Id);
            if (memberCheck == null)
                memberCheck = AddAttendance(member, AttendanceRegisterType.CurrentEvent);
            memberCheck.SetSpeaker();
            return memberCheck;
        }
        public void AddSponsor(Sponsor sponsor, string detail)
        {
            Sponsors.Add(new SponsorEvent
            {
                Detail = detail,
                Sponsor = sponsor
            });
        }
        public void SetLive()
        {
            Live = true;
            StartLiveTime = DateTime.Now;
            AddDomainEvent(new EventLive(this));
        }
        public void EnableGeneralAttendace()
        {
            GeneralAttended = true;
            GeneralAttendedCode = RandomHelper.RandomString(7);
            AddDomainEvent(new EnableGeneralAttendance(Id));
        }
        public void DisableGeneralAttendace()
        {
            GeneralAttended = false;
            AddDomainEvent(new DisableGeneralAttendance(Id));
        }
        public void SetUnLive()
        {
            Live = false;
            EndLiveTime = DateTime.Now;
            AddDomainEvent(new EventUnLive(this));
        }

        public GroupCode CreateGroupCode(string detail)
        {
            var newGroupCode = new GroupCode(detail);
            GroupCodes.Add(newGroupCode);
            return newGroupCode;
        }

        public void Complete(CompleteEvent completeEvent = null, Badge badge = null)
        {
            if (completeEvent == null)
                completeEvent = new CompleteEvent();
            SetUnLive();
            Done = true;
            Attendees.ToList()
                     .ForEach(x =>
                     {

                         if (!x.Attended && !x.NotifiedAbsence)
                             x.NoAttend();
                         if (x.Attended)
                         {
                             if (completeEvent.ThanksAttendees)
                                 AddDomainEvent(new NotifiedAttendedEventEnd(Id, x.MemberId, completeEvent.SendMaterialToAttendees));
                             if (completeEvent.GiveBadgeToAttendees)
                             {
                                 x.Member.AssignBadge(badge);
                                 AddDomainEvent(new AssignedBadgeToMember(x.MemberId, badge.Id));
                             }
                         }

                     });
            if (completeEvent.ThanksSpeakers)
                AddDomainEvent(new NotifiedSpeakersEventEnd(Id));

            if (completeEvent.ThanksSponsors)
                AddDomainEvent(new NotifiedSponsorsEventEnd(Id));

        }
    }
}