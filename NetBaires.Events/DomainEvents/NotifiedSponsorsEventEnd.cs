namespace NetBaires.Events.DomainEvents
{
    public class NotifiedSponsorsEventEnd : IDomainEvents
    {
        public int EventId { get; }

        public NotifiedSponsorsEventEnd(int eventId)
        {
            EventId = eventId;
        }

        public NotifiedSponsorsEventEnd()
        {
            
        }
    }
}