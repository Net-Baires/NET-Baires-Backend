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
        public DateTime Date { get; set; }
        public List<SponsorEvent> Sponsors { get; set; }

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
        public void SetLive()
        {
            Live = true;
            AddDomainEvent(new EventLive(this));
        }
        public void SetUnLive()
        {
            Live = true;
            AddDomainEvent(new EventUnLive(this));
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
}