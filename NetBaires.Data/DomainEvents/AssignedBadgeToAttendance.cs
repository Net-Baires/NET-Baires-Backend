using System;

namespace NetBaires.Data.DomainEvents
{
    public class AssignedBadgeToAttendance : IDomainEvents
    {
        public int MemberId { get; }
        public Badge Badge { get; }

        public AssignedBadgeToAttendance(int memberId, Badge badge)
        {
            MemberId = memberId;
            Badge = badge ?? throw new ArgumentNullException(nameof(badge));
        }
    }
    public class EnableGeneralAttendance : IDomainEvents
    {
        public int EventId { get; }

        public EnableGeneralAttendance(int eventId)
        {
            EventId = eventId;
        }
    }
    public class DisableGeneralAttendance : IDomainEvents
    {
        public int EventId { get; }

        public DisableGeneralAttendance(int eventId)
        {
            EventId = eventId;
        }
    }
    public class EventLive : IDomainEvents
    {
        public Event Event { get; }

        public EventLive(Event @event)
        {
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
        }
    }
    public class EventUnLive : IDomainEvents
    {
        public Event Event { get; }

        public EventUnLive(Event @event)
        {
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
        }
    }
}
