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

    public class GetBadesHandler : IRequestHandler<GetBadesHandler.GetBades, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IBadgesServices _badgesServices;
        private readonly ILogger<GetBadesHandler> _logger;

        public GetBadesHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IMapper mapper,
            IBadgesServices badgesServices,
            IOptions<AssistanceOptions> assistanceOptions,
            ILogger<GetBadesHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            this._badgesServices = badgesServices;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetBades request, CancellationToken cancellationToken)
        {
            var badgesFromUser = await _context.Badges.ToListAsync();

            if (!badgesFromUser.Any())
                return new StatusCodeResult(202);

            var badgesToReturn = _mapper.Map(badgesFromUser, new List<BadgeDetailViewModel>());

            foreach (var badge in badgesToReturn)
                badge.BadgeImageUrl = _badgesServices.GenerateImageUrl(badge.Id);

            return new ObjectResult(badgesToReturn) { StatusCode = 200 };

        }

        public class GetBades : IRequest<IActionResult>
        {
        }

    }
}