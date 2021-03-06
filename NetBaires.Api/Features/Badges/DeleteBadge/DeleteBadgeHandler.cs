﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.DeleteBadge
{
    public class DeleteBadgeHandler : IRequestHandler<DeleteBadgeCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IBadgesServices badgesServices;

        public DeleteBadgeHandler(NetBairesContext context,
            IBadgesServices badgesServices,
            ILogger<DeleteBadgeHandler> logger)
        {
            _context = context;
            this.badgesServices = badgesServices;
        }


        public async Task<IActionResult> Handle(DeleteBadgeCommand request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (badge == null)
                return new StatusCodeResult(404);
            if (!(await _context.Badges.AnyAsync(x => x.Id == request.Id && !x.Members.Any())))
                return new StatusCodeResult(409);

            if (await badgesServices.RemoveAsync(badge.ImageName))
            {
                _context.Remove(badge);
                await _context.SaveChangesAsync();
            }
            return new ObjectResult(request) { StatusCode = 200 };
        }
    }
}