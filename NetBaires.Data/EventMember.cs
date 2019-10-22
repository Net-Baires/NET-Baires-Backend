namespace NetBaires.Data
{
    public class EventMember
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int UserId { get; set; }
        public Member User { get; set; }
    }
}