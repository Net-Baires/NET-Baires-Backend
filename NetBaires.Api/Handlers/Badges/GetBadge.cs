using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Handlers.Badges.Models;
using NetBaires.Api.Models;
using NetBaires.Api.Options;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{

    public class GetBadeHandler : IRequestHandler<GetBadeHandler.GetBadge, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IBadgesServices _badgesServices;
        private readonly ILogger<GetBadeHandler> _logger;

        public GetBadeHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IMapper mapper,
            IBadgesServices badgesServices,
            IOptions<AttendanceOptions> assistanceOptions,
            ILogger<GetBadeHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _badgesServices = badgesServices;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetBadge request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.BadgeId);
            if (badge == null)
                return new StatusCodeResult(404);
            var badgeToResponse = _mapper.Map(badge, new GetBadgeResponse());
            badgeToResponse.BadgeImageUrl = _badgesServices.GenerateImageUrl(badge);
            badgeToResponse.BadgeUrl = _badgesServices.GenerateDetailUrl(badge);
            return new ObjectResult(badgeToResponse) { StatusCode = 200 }; ;
        }

        public class GetBadge : IRequest<IActionResult>
        {
            public GetBadge(int badgeId)
            {
                BadgeId = badgeId;
            }

            public int BadgeId { get; }
        }

        public class GetBadgeResponse
        {
            public int Id { get; set; }
            public string BadgeUrl { get; set; }
            public DateTime Created { get; set; }
            public string BadgeImageUrl { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

        }
        public class GetBadgeResponseProfile : Profile
        {
            public GetBadgeResponseProfile()
            {
                CreateMap<Badge, GetBadgeResponse>();
            }
        }
    }
}