﻿using System;

namespace NetBaires.Data
{
    public class Attendance
    {
        public void Attend()
        {
            Attended = true;
            DidNotAttend = false;
            NotifiedAbsence = false;
            AttendedTime = DateTime.UtcNow;
        }

        public void NoAttend()
        {
            Attended = false;
            DidNotAttend = true;
            NotifiedAbsence = false;
        }
        public void NotifyAbsence()
        {
            Attended = false;
            DidNotAttend = true;
            NotifiedAbsence = true;
        }

        public void SetDoNotKnow()
        {
            Attended = false;
            DidNotAttend = false;
            DoNotKnow = true;
            NotifiedAbsence = false;
        }


        public int EventId { get; set; }
        public Event Event { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime AttendedTime { get; set; }
        public bool Organizer { get; set; } = false;
        public bool Speaker { get; set; } = false;
        public bool DidNotAttend { get; set; } = false;
        public bool Attended { get; set; } = false;
        public bool NotifiedAbsence { get; set; } = false;
        public bool DoNotKnow { get; set; } = false;
        public Attendance(int memberId, int eventId)
        {
            MemberId = memberId;
            EventId = eventId;
            DidNotAttend = true;
        }

        public Attendance()
        {

        }
        public Attendance(Member member, Event eventToAdd, bool attended)
        {
            if (member.Id == 0)
                Member = member;
            else
                MemberId = member.Id;
            EventId = eventToAdd.Id;
            if (attended)
                Attend();
            else
                NoAttend();

        }
        public Attendance(Member member, Event eventToAdd)
        {
            if (member.Id == 0)
                Member = member;
            else
                MemberId = member.Id;
            EventId = eventToAdd.Id;

        }
    }
}