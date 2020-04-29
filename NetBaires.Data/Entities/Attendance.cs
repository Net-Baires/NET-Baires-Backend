using System;

namespace NetBaires.Data.Entities
{
    public enum AttendanceRegisterType
    {
        //El usuario se registro en el sitio de Meetup o EventBrite y fue sincronizado
        ExternalPage,
        //EL usuario no estaba registrado en otro sitio y llego al evento directo.
        CurrentEvent,
        //El usuario se registro al evento mediante la app de NET-Baires
        InApp
    }

    public class Attendance
    {
        public void Attend()
        {
            Attended = true;
            DidNotAttend = false;
            DoNotKnow = false;
            NotifiedAbsence = false;
            AttendedTime = DateTime.UtcNow;
        }

        public void NoAttend()
        {
            Attended = false;
            DoNotKnow = false;

            DidNotAttend = true;
            NotifiedAbsence = false;
        }
        public void NotifyAbsence()
        {
            Attended = false;
            DidNotAttend = true;
            DoNotKnow = false;

            NotifiedAbsence = true;
        }

        public void SetDoNotKnow()
        {
            Attended = false;
            DidNotAttend = false;
            DoNotKnow = true;
            NotifiedAbsence = false;
        }
        public void SetSpeaker()
        {
            Attend();
            Speaker = true;
        }
        public void RemoveSpeaker()
        {
            Speaker = false;
        }

        public void SetOrganizer()
        {
            Attend();
            Organizer = true;
        }
        public void RemoveOrganizer()
        {
            Organizer = false;
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
        public AttendanceRegisterType AttendanceRegisterType { get; set; }
        public Attendance(int memberId, int eventId, AttendanceRegisterType attendanceRegisterType)
        {
            MemberId = memberId;
            EventId = eventId;
            AttendanceRegisterType = attendanceRegisterType;
            DidNotAttend = true;
        }

        public Attendance()
        {

        }
        public Attendance(Member member, Event eventToAdd, bool attended, AttendanceRegisterType attendanceRegisterType)
        {
            AttendanceRegisterType = attendanceRegisterType;
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
        public Attendance(Member member, Event eventToAdd, AttendanceRegisterType attendanceRegisterType)
        {
            AttendanceRegisterType = attendanceRegisterType;
            if (member.Id == 0)
                Member = member;
            else
                MemberId = member.Id;
            EventId = eventToAdd.Id;

        }
    }
}