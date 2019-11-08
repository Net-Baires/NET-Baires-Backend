using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Badges.UpdateBadge
{
    public class UpdateBadgeCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public IFormFileCollection ImageFiles { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}