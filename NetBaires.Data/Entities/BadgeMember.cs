using System;

namespace NetBaires.Data
{
    public class BadgeMember
    {
        public int BadgeId { get; set; }
        public Badge Badge { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public DateTime AssignmentDate { get; set; } = DateTime.Now;
    }
}