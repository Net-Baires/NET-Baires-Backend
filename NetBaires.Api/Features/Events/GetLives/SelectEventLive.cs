using System;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{
    public class SelectEventLive
    {
        public Event Event { get; set; }
        public bool Registered { get; set; }

        public SelectEventLive(Event @event, bool alreadyRegistered)
        {
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
            Registered = alreadyRegistered;
        }
    }
}