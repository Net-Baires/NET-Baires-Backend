using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.GetBadge
{
    public class SearchMemberQuery : IRequest<IActionResult>
    {
        public string Query { get; set; }
    }
}