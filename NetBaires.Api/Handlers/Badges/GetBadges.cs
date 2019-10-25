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

    public class GetBadgesHandler : IRequestHandler<GetBadgesHandler.GetBages, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IBadgesServices _badgesServices;
        private readonly ILogger<GetBadgesHandler> _logger;

        public GetBadgesHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IMapper mapper,
            IBadgesServices badgesServices,
            IOptions<AssistanceOptions> assistanceOptions,
            ILogger<GetBadgesHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            this._badgesServices = badgesServices;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetBages request, CancellationToken cancellationToken)
        {
            var badgesFromUser = await _context.Badges.ToListAsync();

            if (!badgesFromUser.Any())
                return new StatusCodeResult(204);

            var badgesToReturn = _mapper.Map(badgesFromUser, new List<GetBadgeResponse>());

            foreach (var badge in badgesToReturn)
            {
                badge.BadgeUrl = _badgesServices.GenerateDetailUrl(badge.Id);
                badge.BadgeImageUrl = _badgesServices.GenerateImageUrl(badge.Id);
            }

            return new ObjectResult(badgesToReturn) { StatusCode = 200 };

        }

        public class GetBages : IRequest<IActionResult>
        {
        }

        public class GetBadgeResponse
        {
            public int Id { get; set; }
            public DateTime Created { get; set; }
            public string BadgeImageUrl { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string BadgeUrl { get; set; }
        }
        public class GetBadgeResponseProfile : Profile
        {
            public GetBadgeResponseProfile()
            {
                CreateMap<Badge, GetBadgeResponse>();
                CreateMap<BadgeMember, GetBadgeResponse>()
                  .ForMember(dest => dest.BadgeImageUrl, opt => opt.MapFrom(src => src.BadgeUrl))
                  .ForAllMembers(
                  opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
                CreateMap<Badge, GetBadgeResponse>()
                      .ForMember(dest => dest.BadgeImageUrl, opt => opt.MapFrom(src => src.ImageName))
                .ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null));
            }
        }
    }
}