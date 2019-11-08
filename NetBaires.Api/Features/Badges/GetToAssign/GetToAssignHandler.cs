using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetToAssign
{

    public class GetToAssignHandler : IRequestHandler<GetToAssignCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToAssignHandler> _logger;

        public GetToAssignHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IMapper mapper,
            ILogger<GetToAssignHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetToAssignCommand request, CancellationToken cancellationToken)
        {
            var badgesFromUser = await _context.BadgeMembers.Where(x => x.MemberId == request.MemberId).ToListAsync(cancellationToken: cancellationToken);
            var allBadges = await _context.Badges.ToListAsync(cancellationToken: cancellationToken);

            var badgesAssigned = allBadges.Where(x => badgesFromUser.Any(s => x.Id == s.BadgeId));
            var badgesNotAssigned = allBadges.Where(x => !badgesFromUser.Any(s => x.Id == s.BadgeId));

            var response = badgesNotAssigned.Aggregate(badgesAssigned.Aggregate(new List<BadgeAssignResponse>(),
                     (accum, item) => ReduceBadge(item, accum, true)),
                 (accum, item) => ReduceBadge(item, accum, false));


            return new ObjectResult(response.OrderByDescending(x => x.Id)) { StatusCode = 200 };

        }

        private List<BadgeAssignResponse> ReduceBadge(Badge item, List<BadgeAssignResponse> accum, bool assigned)
        {
            var returnValue = _mapper.Map(item, new BadgeAssignResponse());
            returnValue.Assigned = assigned;
            accum.Add(returnValue);
            return accum;
        }
    }
}