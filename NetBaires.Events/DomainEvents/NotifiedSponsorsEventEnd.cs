namespace NetBaires.Events.DomainEvents
{
    public class NotifiedSponsorsEventEnd : DomainEvents<NotifiedSponsorsEventEnd>
    {
        public int EventId { get; set; }

        public NotifiedSponsorsEventEnd(int eventId)
        {
            EventId = eventId;
        }

        public NotifiedSponsorsEventEnd()
        {

        }

        public override int GetHashCode() =>
            EventId;
    }
}