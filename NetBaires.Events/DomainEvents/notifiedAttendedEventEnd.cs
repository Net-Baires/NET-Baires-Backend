namespace NetBaires.Events.DomainEvents
{
    public class NotifiedAttendedEventEnd : IDomainEvents
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
    }
}