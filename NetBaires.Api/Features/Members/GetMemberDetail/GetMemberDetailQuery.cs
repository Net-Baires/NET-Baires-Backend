using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Members.GetMemberDetail
{
    public class GetMemberDetailQuery : IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
}