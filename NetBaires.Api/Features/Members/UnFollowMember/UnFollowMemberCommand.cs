using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Members.UnFollowMember
{
    public class UnFollowMemberCommand : IRequest<IActionResult>
    {
        public int MemberId { get;  }

        public UnFollowMemberCommand(int memberId)
        {
            MemberId = memberId;
        }
    }
}