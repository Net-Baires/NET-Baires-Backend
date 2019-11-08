using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetBadges
{

    public class GetBadgesHandler : IRequestHandler<GetBagesCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IBadgesServices _badgesServices;
        private readonly ILogger<GetBadgesHandler> _logger;

        public GetBadgesHandler(NetBairesContext context,
            IMapper mapper,
            IBadgesServices badgesServices,
            ILogger<GetBadgesHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _badgesServices = badgesServices;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetBagesCommand request, CancellationToken cancellationToken)
        {
            var badgesFromUser = await _context.Badges.ToListAsync();

            if (!badgesFromUser.Any())
                return new StatusCodeResult(204);

            var listBadGeReturn = new List<GetBadgeResponse>();
            foreach (var badge in badgesFromUser)
            {
                var badgeToReturn = _mapper.Map(badge, new GetBadgeResponse());
                badgeToReturn.BadgeUrl = _badgesServices.GenerateDetailUrl(badge.Id);
                badgeToReturn.BadgeImageUrl = _badgesServices.GenerateImageUrl(badge);
                listBadGeReturn.Add(badgeToReturn);
            }

            return new ObjectResult(listBadGeReturn) { StatusCode = 200 };
            }
        }
}