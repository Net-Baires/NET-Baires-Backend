using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetToAssign
{

    public class GetMembersInBadgeHandler : IRequestHandler<GetMembersInBadgeQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToAssignHandler> _logger;

        public GetMembersInBadgeHandler(NetBairesContext context,
            IMapper mapper,
            ILogger<GetToAssignHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetMembersInBadgeQuery request, CancellationToken cancellationToken)
        {
            var members = await _context.BadgeMembers.Where(x => x.BadgeId == request.BadgeId).Select(x=> x.Member).ToListAsync();
            if (!members.Any())
                return new StatusCodeResult(204);

            return new ObjectResult(_mapper.Map(members, new List<MemberDetailViewModel>())) { StatusCode = 200 };
        }
    }
}