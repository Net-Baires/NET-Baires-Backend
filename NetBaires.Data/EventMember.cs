using System;

namespace NetBaires.Data
{
    public class EventMember
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
        public bool Attended { get; protected set; }
        protected EventMember(int memberId, int eventId, bool attended)
        {
            MemberId = memberId;
            EventId = eventId;
            Attended = attended;
        }


        protected EventMember()
        {

        }
        public void Inform(bool attended)
        {
            Attended = attended;
        }
        public void Attend()
        {
            Attended = true;
        }

        public void NoAttend()
        {
            Attended = true;
        }
        public static EventMember Inform(int memberId, int eventId, bool attended)
        {
            return new EventMember(memberId, eventId, attended);
        }
        public static EventMember Attend(int memberId, int eventId)
        {
            return new EventMember(memberId, eventId, true);
        }
        public static EventMember NoAttend(int memberId, int eventId)
        {
            return new EventMember(memberId, eventId, false);
        }
    }
}