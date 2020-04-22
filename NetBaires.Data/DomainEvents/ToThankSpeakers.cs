using NetBaires.Events.DomainEvents;

namespace NetBaires.Data.DomainEvents
{
    public class ToThankSpeakers : IDomainEvents
    {
        public int EventId { get; }

        public ToThankSpeakers(int eventId)
        {
            EventId = eventId;
        }
    }
}