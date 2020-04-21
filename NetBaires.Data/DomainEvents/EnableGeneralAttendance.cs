using NetBaires.Events.DomainEvents;

namespace NetBaires.Data.DomainEvents
{
    public class EnableGeneralAttendance : IDomainEvents
    {
        public int EventId { get; }

        public EnableGeneralAttendance(int eventId)
        {
            EventId = eventId;
        }
    }
}