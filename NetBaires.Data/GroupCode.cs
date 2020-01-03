using System.Collections.Generic;
using System.Linq;

namespace NetBaires.Data
{
    public class GroupCode : Entity
    {
        public string Code { get; protected set; }
        public string Detail { get; set; }
        public bool Open { get; protected set; } = true;
        public List<Member> Members { get; set; } = new List<Member>();
        public int EventId { get; set; }

        public Event Event { get; set; }

        public GroupCode(string detail)
        {
            Detail = detail;
            Code = RandomHelper.RandomString(8);
        }

        public void AddMember(Member member,string code)
        {
            if (Members.All(x => x.Id != member.Id) && code.ToUpper() == Code.ToUpper())
                Members.Add(member);
        }
    }
}