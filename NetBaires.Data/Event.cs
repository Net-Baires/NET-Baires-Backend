using System;
using System.Collections.Generic;
using System.Linq;
using NetBaires.Data.DomainEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NetBaires.Data
{
    public class Event : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public EventPlatform Platform { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public List<Attendance> Attendees { get; set; } = new List<Attendance>();
        public string EventId { get; set; }
        public bool Done { get; protected set; } = false;
        public bool Live { get; set; } = false;
        public bool GeneralAttended { get; set; } = false;
        public string GeneralAttendedCode { get; set; }
        public DateTime? StartLiveTime { get; set; }
        public DateTime? EndLiveTime { get; set; }
        public DateTime Date { get; set; }
        public List<SponsorEvent> Sponsors { get; set; } = new List<SponsorEvent>();
        public List<GroupCode> GroupCodes { get; protected set; } = new List<GroupCode>();
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
                    AddDomainEvent(new AssignedBadgeToAttendance(item.Member, badge));
                }
            return DomainResponse.Ok();
        }

        public void SetDone()
        {
            Done = true;
        }

        public Attendance AddAttendance(Member member)
        {
            var attendance = new Attendance(member, this);
            Attendees.Add(attendance);
            return attendance;
        }
        public Attendance Attended(Member member)
        {
            var memberCheck = Attendees.FirstOrDefault(x => x.MemberId == member.Id);
            if (memberCheck != null)
                memberCheck.Attend();
            else
            {
                memberCheck = AddAttendance(member);
                memberCheck.Attend();
            }
            return memberCheck;
        }
        public Attendance AddSpeaker(Member member)
        {
            var memberCheck = Attendees.FirstOrDefault(x => x.MemberId == member.Id);
            if (memberCheck == null)
                memberCheck = AddAttendance(member);
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

        public void Complete()
        {
            Done = true;
            Attendees.ToList()
                     .ForEach(x =>
                     {
                         if (!x.Attended
                         &&
                         !x.NotifiedAbsence)
                             x.NoAttend();
                     });


        }
    }
    public static class RandomHelper
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}