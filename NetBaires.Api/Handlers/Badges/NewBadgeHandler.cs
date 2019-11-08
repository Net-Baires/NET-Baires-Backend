using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{
    public enum BadgeImageName
    {
        Badge,
        SimpleBadge
    }
    public class NewBadgeHandler : IRequestHandler<NewBadgeHandler.NewBadge, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IBadgesServices badgesServices;
        private readonly IMapper _mapper;
        private readonly ILogger<NewBadgeHandler> logger;

        public NewBadgeHandler(NetBairesContext context,
               IBadgesServices badgesServices,
            IMapper mapper,
            ILogger<NewBadgeHandler> logger)
        {
            _context = context;
            this.badgesServices = badgesServices;
            this._mapper = mapper;
            this.logger = logger;
        }


        public async Task<IActionResult> Handle(NewBadgeHandler.NewBadge request, CancellationToken cancellationToken)
        {
            var newBadge = _mapper.Map(request, new Badge());
            if (request.ImageFiles != null)
            {
                foreach (var item in request.ImageFiles)
                {
                    if (item.Headers["BadgeType"] == BadgeImageName.Badge.ToString())
                    {
                        var badgeCreateResponse = await badgesServices.CreateAsync(item);
                        if (badgeCreateResponse == null)
                            return new StatusCodeResult(400);
                        newBadge.ImageName = badgeCreateResponse.FileDetail.Name;
                    }
                    else if (item.Headers["BadgeType"] == BadgeImageName.SimpleBadge.ToString())
                    {
                        var badgeCreateResponse = await badgesServices.CreateAsync(item);
                        if (badgeCreateResponse == null)
                            return new StatusCodeResult(400);
                        newBadge.SimpleImageName = badgeCreateResponse.FileDetail.Name;
                    }
                }
            }
            await _context.Badges.AddAsync(newBadge);
            await _context.SaveChangesAsync();


            return new StatusCodeResult(204);

        }

      
        public class NewBadge : IRequest<IActionResult>
        {
            public IFormFileCollection ImageFiles { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

        }
        public class NewBadgeProfile : Profile
        {
            public NewBadgeProfile()
            {
                CreateMap<NewBadge, Badge>();
            }
        }
    }
}