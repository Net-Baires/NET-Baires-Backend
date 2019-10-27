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
using NetBaires.Api.Models;
using NetBaires.Api.Options;
using NetBaires.Api.Services;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{

    public class GetToAssignHandler : IRequestHandler<GetToAssignHandler.GetToAssign, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IBadgesServices _badgesServices;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToAssignHandler> _logger;

        public GetToAssignHandler(ICurrentUser currentUser,
            NetBairesContext context,
          IBadgesServices badgesServices,
            IMapper mapper,
            ILogger<GetToAssignHandler> logger)
        {
            _context = context;
            this._badgesServices = badgesServices;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetToAssign request, CancellationToken cancellationToken)
        {
            var badgesFromUser = await _context.BadgeMembers.Where(x => x.MemberId == request.MemberId).ToListAsync(cancellationToken: cancellationToken);
            var allBadGes = await _context.Badges.ToListAsync(cancellationToken: cancellationToken);

            var badGesAssigned = allBadGes.Where(x => badgesFromUser.Any(s => x.Id == s.BadgeId));
            var badGesNotAssigned = allBadGes.Where(x => !badgesFromUser.Any(s => x.Id == s.BadgeId));

            var response = badGesNotAssigned.Aggregate(badGesAssigned.Aggregate(new List<BadgeAssignResponse>(),
                     (accum, item) => ReduceBadge(item, accum, true)),
                 (accum, item) => ReduceBadge(item, accum, false));


            return new ObjectResult(response.OrderByDescending(x => x.Id)) { StatusCode = 200 };

        }

        private List<BadgeAssignResponse> ReduceBadge(Badge item, List<BadgeAssignResponse> accum, bool assigned)
        {
            var returnValue = _mapper.Map(item, new BadgeAssignResponse());
            returnValue.Assigned = assigned;
            returnValue.BadgeUrl = _badgesServices.GenerateDetailUrl(item.Id);
            returnValue.BadgeImageUrl = _badgesServices.GenerateImageUrl(item.Id);
            accum.Add(returnValue);
            return accum;
        }


        public class GetToAssign : IRequest<IActionResult>
        {
            public GetToAssign(int memberId)
            {
                MemberId = memberId;
            }

            public int MemberId { get; }
        }

        public class BadgeAssignResponse
        {
            public int Id { get; set; }
            public string BadgeUrl { get; set; }
            public string BadgeImageUrl { get; set; }
            public string Name { get; set; }
            public DateTime Created { get; set; }
            public bool Assigned { get; set; }
        }
        public class BadgeAssignResponseProfile : Profile
        {
            public BadgeAssignResponseProfile()
            {
                CreateMap<Badge, BadgeAssignResponse>();
            }
        }
    }
}