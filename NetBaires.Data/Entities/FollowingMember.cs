using System;

namespace NetBaires.Data.Entities
{
    public class FollowedMember : Entity
    {
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public Member Followed { get; set; }
        public DateTime FollowingDate { get; set; }

        public FollowedMember(Member member, DateTime followedMember)
        {
            Followed = member;
            FollowingDate = followedMember;
        }

        public FollowedMember()
        {

        }
    }
}