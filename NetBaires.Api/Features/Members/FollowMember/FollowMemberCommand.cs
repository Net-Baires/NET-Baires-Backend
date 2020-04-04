using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Members.FollowMember
{
    public class FollowMemberCommand : IRequest<IActionResult>
    {
        public int MemberId { get;  }

        public FollowMemberCommand(int memberId)
        {
            MemberId = memberId;
        }
    }
}