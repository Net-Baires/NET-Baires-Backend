using NetBaires.Events.DomainEvents;

namespace NetBaires.Data.DomainEvents
{
    public class ToThankAttended : IDomainEvents
    {
        public int EventId { get; }
        public int MemberId { get; set; }
        public bool SendMaterialToAttendee { get; set; }

        public ToThankAttended(int eventId, int memberId, bool sendMaterialToAttendee)
        {
            EventId = eventId;
            MemberId = memberId;
            SendMaterialToAttendee = sendMaterialToAttendee;
        }
    }
}