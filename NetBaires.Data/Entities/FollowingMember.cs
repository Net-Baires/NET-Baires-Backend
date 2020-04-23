#nullable enable
using System;

namespace NetBaires.Data.Entities
{
    public class FollowingMember :Entity
    {
        public int? MemberId { get; set; }
        public Member? Member { get; set; }
        public Member? Following { get; set; }
        public int? FollowingId { get; set; }
        public DateTime FollowingDate { get; set; }

        public FollowingMember(Member member, Member followingMember, DateTime followingDate)
        {
            MemberId = member.Id;
            Member = member;
            FollowingId = followingMember.Id;
            Following = followingMember;
            FollowingDate = followingDate;
        }

        public FollowingMember()
        {

        }
    }
}