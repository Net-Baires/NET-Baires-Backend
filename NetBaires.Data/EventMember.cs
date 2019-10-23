using System;

namespace NetBaires.Data
{
    [Flags]
    public enum EventMemberStatus
    {
        Registered = 0,
        DidNotAttend = 1,
        Attended = 2,
        NotifyAbsence = 3

    }
    public class EventMember
    {
        public void Attend()
        {
            Status &= ~EventMemberStatus.DidNotAttend;
            Status = Status & EventMemberStatus.DidNotAttend;
        }

        public void NoAttend()
        {
            Status &= ~EventMemberStatus.Attended;
            Status = Status & EventMemberStatus.DidNotAttend;
        }
        public void NotifyAbsence()
        {
            Status &= ~EventMemberStatus.Attended;
            Status = Status & EventMemberStatus.NotifyAbsence;
        }


        public int EventId { get; set; }
        public Event Event { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
        public EventMemberStatus Status { get; set; }
        public EventMember(int memberId, int eventId, EventMemberStatus status)
        {
            MemberId = memberId;
            EventId = eventId;
            Status = status;
        }
        public EventMember(int memberId, int eventId)
        {
            MemberId = memberId;
            EventId = eventId;
            Status = EventMemberStatus.Registered | EventMemberStatus.DidNotAttend;
        }

        public EventMember()
        {

        }
    }
}