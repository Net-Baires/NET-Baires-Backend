namespace NetBaires.Data
{
    public class GroupCodeMember : Entity
    {
        public int GroupCodeId { get; set; }
        public GroupCode GroupCode { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}