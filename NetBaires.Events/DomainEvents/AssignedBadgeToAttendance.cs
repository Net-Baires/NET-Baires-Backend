namespace NetBaires.Events.DomainEvents
{
    public class AssignedBadgeToAttendance : IDomainEvents
    {
        public int MemberId { get; set; }
        public int BadgeId { get; set; }
        public AssignedBadgeToAttendance(int memberId, int badgeId)
        {
            MemberId = memberId;
            BadgeId = badgeId;
        }

        public AssignedBadgeToAttendance()
        {
            
        }
    }
}
