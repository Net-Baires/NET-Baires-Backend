using NetBaires.Events.DomainEvents;

namespace NetBaires.Data.DomainEvents
{
    public class DisableGeneralAttendance : IDomainEvents
    {
        public int EventId { get; }

        public DisableGeneralAttendance(int eventId)
        {
            EventId = eventId;
        }
    }
}