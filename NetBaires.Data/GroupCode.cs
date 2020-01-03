using System.Collections.Generic;
using System.Linq;

namespace NetBaires.Data
{
    public class GroupCode : Entity
    {
        public string Code { get; set; }
        public string Detail { get; set; }
        public bool Open { get; set; } = true;
        public List<GroupCodeMember> Members { get; set; } = new List<GroupCodeMember>();
        public int EventId { get; set; }

        public Event Event { get; set; }

        public GroupCode(string detail)
        {
            Open = false;
            Detail = detail;
            Code = RandomHelper.RandomString(8);
        }

        public void AddMember(Member member, string code)
        {
            if (Members.All(x => x.MemberId != member.Id) && code.ToUpper() == Code.ToUpper())
                Members.Add(new GroupCodeMember
                {
                    Member = member,
                    GroupCode = this
                });
        }
    }
}