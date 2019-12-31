using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Badges.DeleteBadge
{
    public class DeleteBadgeCommand : IRequest<IActionResult>
    {
        public DeleteBadgeCommand(int id)
        {
            Id = id;
        }
        public int Id { get; }
    }
}