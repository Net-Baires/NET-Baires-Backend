using System;
using NetBaires.Data.Entities;
using NetBaires.Events.DomainEvents;

namespace NetBaires.Data.DomainEvents
{
    public class EventUnLive : IDomainEvents
    {
        public int EventId { get; }

        public EventUnLive(Event @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));
            EventId = @event.Id;
        }
    }
}