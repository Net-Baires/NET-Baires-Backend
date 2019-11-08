using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.GetToAssign
{
    public class GetToAssignCommand : IRequest<IActionResult>
    {
        public GetToAssignCommand(int memberId)
        {
            MemberId = memberId;
        }

        public int MemberId { get; }
    }
}