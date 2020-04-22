using NetBaires.Events.DomainEvents;

namespace NetBaires.Data.DomainEvents
{
    public class ToThankSponsors : IDomainEvents
    {
        public int EventId { get; }

        public ToThankSponsors(int eventId)
        {
            EventId = eventId;
        }
    }
}