using System;
using NetBaires.Events.DomainEvents;

namespace NetBaires.Data.DomainEvents
{
    public class EventLive : IDomainEvents
    {
        public int EventId { get; }

        public EventLive(Event @event)
        {
            if (@event == null)throw new ArgumentNullException(nameof(@event));
            EventId = @event.Id;
        }
    }
}