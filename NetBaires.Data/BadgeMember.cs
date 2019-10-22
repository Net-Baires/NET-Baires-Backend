namespace NetBaires.Data
{
    public class BadgeMember
    {
        public int BadgeId { get; set; }
        public Badge Badge { get; set; }
        public int UserId { get; set; }
        public string BadgeUrl { get; set; }
        public Member User { get; set; }
    }
}