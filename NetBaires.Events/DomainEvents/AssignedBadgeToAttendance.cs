namespace NetBaires.Events.DomainEvents
{
    public class AssignedBadgeToMember : DomainEvents<AssignedBadgeToMember>
    {
        public int MemberId { get; set; }
        public int BadgeId { get; set; }
        public AssignedBadgeToMember(int memberId, int badgeId)
        {
            MemberId = memberId;
            BadgeId = badgeId;
        }


        public AssignedBadgeToMember()
        {

        }

        public override int GetHashCode() =>
            BadgeId ^ MemberId;
    }
}
