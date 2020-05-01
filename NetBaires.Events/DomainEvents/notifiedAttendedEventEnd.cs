namespace NetBaires.Events.DomainEvents
{
    public class NotifiedAttendedEventEnd : DomainEvents<NotifiedAttendedEventEnd>
    {
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public bool SendMaterialToAttendee { get; set; }

        public NotifiedAttendedEventEnd(int eventId, int memberId, bool sendMaterialToAttendee)
        {
            EventId = eventId;
            MemberId = memberId;
            SendMaterialToAttendee = sendMaterialToAttendee;
        }

        public NotifiedAttendedEventEnd()
        {

        }

        public override int GetHashCode() =>
            EventId ^ MemberId;
    }
}