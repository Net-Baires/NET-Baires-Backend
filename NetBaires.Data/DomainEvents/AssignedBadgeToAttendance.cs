using System;
using System.Collections.Generic;
using System.Text;

namespace NetBaires.Data.DomainEvents
{
    public class AssignedBadgeToAttendance : IDomainEvents
    {
        public Member Member { get; }
        public Badge Badge { get; }

        public AssignedBadgeToAttendance(Member member, Badge badge)
        {
            Member = member ?? throw new ArgumentNullException(nameof(member));
            Badge = badge ?? throw new ArgumentNullException(nameof(badge));
        }
    }


}
