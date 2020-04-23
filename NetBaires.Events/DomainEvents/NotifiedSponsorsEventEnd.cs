namespace NetBaires.Events.DomainEvents
{
    public class NotifiedSponsorsEventEnd : IDomainEvents
    {
        public int EventId { get; set; }

        public NotifiedSponsorsEventEnd(int eventId)
        {
            EventId = eventId;
        }

        public NotifiedSponsorsEventEnd()
        {

        }
    }
}