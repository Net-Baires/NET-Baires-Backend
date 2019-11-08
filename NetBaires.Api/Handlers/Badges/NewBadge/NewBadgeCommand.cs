using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Badges.NewBadge
{
    public class NewBadgeCommand : IRequest<IActionResult>
    {
        public IFormFileCollection ImageFiles { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}