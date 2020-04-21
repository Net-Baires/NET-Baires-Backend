using System;

namespace NetBaires.Data.DomainEvents
{
    public class AssignedBadgeToAttendance : IDomainEvents
    {
        public int MemberId { get; set; }
        public int BadgeId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public AssignedBadgeToAttendance(int memberId, Badge badge)
        {
            MemberId = memberId;
            BadgeId = badge.Id;
            Name = badge.Name;
            ImageUrl = badge.ImageUrl;
        }

        public AssignedBadgeToAttendance()
        {
            
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
        public int EventId { get; }

        public EventLive(Event @event)
        {
            if (@event == null)throw new ArgumentNullException(nameof(@event));
            EventId = @event.Id;
        }
    }
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
