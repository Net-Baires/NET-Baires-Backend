using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.GetBadges
{
    public class GetBagesCommand : IRequest<IActionResult>
    {
    }
}