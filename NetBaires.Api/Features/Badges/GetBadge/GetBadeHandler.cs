using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Options;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetBadge
{

    public class GetBadeHandler : IRequestHandler<GetBadgeQuery, IActionResult>
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


        public async Task<IActionResult> Handle(GetBadgeQuery request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.BadgeId);
            if (badge == null)
                return new StatusCodeResult(404);
            var badgeToResponse = _mapper.Map(badge, new GetBadgeResponse());
            badgeToResponse.BadgeImageUrl = _badgesServices.GenerateImageUrl(badge);
            badgeToResponse.BadgeUrl = _badgesServices.GenerateDetailUrl(badge);
            return new ObjectResult(badgeToResponse) { StatusCode = 200 }; ;
        }
    }
}