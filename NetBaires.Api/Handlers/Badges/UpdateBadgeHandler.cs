using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{
    public class UpdateBadgeHandler : IRequestHandler<UpdateBadgeHandler.UpdateBadge, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IBadgesServices badgesServices;
        private readonly ILogger<GetToAssignHandler> _logger;

        public UpdateBadgeHandler(NetBairesContext context,
            IMapper mapper,
            IBadgesServices badgesServices,
            ILogger<UpdateBadgeHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            this.badgesServices = badgesServices;
        }


        public async Task<IActionResult> Handle(UpdateBadge request, CancellationToken cancellationToken)
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


        public class UpdateBadge : IRequest<IActionResult>
        {
            public int Id { get; set; }
            public IFormFileCollection ImageFiles { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class UpdateBadgeProfile : Profile
        {
            public UpdateBadgeProfile()
            {
                CreateMap<UpdateBadge, Badge>().ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            }
        }
    }
}