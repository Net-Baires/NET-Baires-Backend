namespace NetBaires.Events.DomainEvents
{
    public class notifiedAttendedEventEnd : IDomainEvents
    {
        public int EventId { get; }
        public int MemberId { get; set; }
        public bool SendMaterialToAttendee { get; set; }

        public notifiedAttendedEventEnd(int eventId, int memberId, bool sendMaterialToAttendee)
        {
            EventId = eventId;
            MemberId = memberId;
            SendMaterialToAttendee = sendMaterialToAttendee;
        }

        public notifiedAttendedEventEnd()
        {
            
        }
    }
}