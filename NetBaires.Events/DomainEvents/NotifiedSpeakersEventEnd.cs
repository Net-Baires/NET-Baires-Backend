namespace NetBaires.Events.DomainEvents
{
    public class NotifiedSpeakersEventEnd : IDomainEvents
    {
        public int EventId { get; }

        public NotifiedSpeakersEventEnd(int eventId)
        {
            EventId = eventId;
        }

        public NotifiedSpeakersEventEnd()
        {
            
        }
    }
}