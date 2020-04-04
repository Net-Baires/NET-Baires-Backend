using System;

namespace NetBaires.Data.Entities
{
    public class FollowingMember : Entity
    {
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public Member Following { get; set; }
        public int FollowingId { get; set; }
        public DateTime FollowingDate { get; set; }

        public FollowingMember(Member member, DateTime followingMember)
        {
            Following = member;
            FollowingDate = followingMember;
        }

        public FollowingMember()
        {

        }
    }
}