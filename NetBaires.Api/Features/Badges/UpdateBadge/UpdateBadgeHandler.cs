﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Badges.NewBadge;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.UpdateBadge
{
    public class UpdateBadgeHandler : IRequestHandler<UpdateBadgeCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IBadgesServices badgesServices;

        public UpdateBadgeHandler(NetBairesContext context,
            IMapper mapper,
            IBadgesServices badgesServices)
        {
            _context = context;
            _mapper = mapper;
            this.badgesServices = badgesServices;
        }


        public async Task<IActionResult> Handle(UpdateBadgeCommand request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (badge == null)
                return new StatusCodeResult(404);
            _mapper.Map(request, badge);


            if (request.ImageFiles != null)
            {
                foreach (var item in request.ImageFiles)
                {
                    if (item.Headers["BadgeType"] == BadgeImageName.Badge.ToString())
                    {
                        var badgeCreateResponse = await badgesServices.ReplaceAsync(item, badge.ImageName);
                        if (badgeCreateResponse == null)
                            return new StatusCodeResult(400);
                        badge.ImageName = badgeCreateResponse.FileDetail.Name;
                    }
                    else if (item.Headers["BadgeType"] == BadgeImageName.SimpleBadge.ToString())
                    {
                        var badgeCreateResponse = await badgesServices.ReplaceAsync(item, badge.SimpleImageName);
                        if (badgeCreateResponse == null)
                            return new StatusCodeResult(400);
                        badge.SimpleImageName = badgeCreateResponse.FileDetail.Name;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return new StatusCodeResult(204);

        }
    }
}