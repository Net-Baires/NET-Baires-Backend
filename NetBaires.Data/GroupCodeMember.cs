namespace NetBaires.Data
{
    public class GroupCodeMember : Entity
    {
        public int GroupCodeId { get; set; }
        public GroupCode GroupCode { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public bool Winner { get; set; }
        public int WinnerPosition { get; set; }
        public void SetAsWinner(int position)
        {
            Winner = true;
            WinnerPosition = position;
        }
    }
}