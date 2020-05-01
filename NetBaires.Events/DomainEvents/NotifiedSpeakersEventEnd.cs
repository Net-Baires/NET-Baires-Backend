namespace NetBaires.Events.DomainEvents
{
    public class NotifiedSpeakersEventEnd :  DomainEvents<NotifiedSpeakersEventEnd>
    {
        public int EventId { get; set; }

        public NotifiedSpeakersEventEnd(int eventId)
        {
            EventId = eventId;
        }

        public NotifiedSpeakersEventEnd()
        {

        }

        public override int GetHashCode() =>
            EventId;
    }
}