using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Badges.GetToAssign;
using NetBaires.Api.Features.Badges.Models;
using NetBaires.Api.Features.Badges.UpdateBadge;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.AssignMembersToBadge
{
    public class GetBadgesFromEmailHandler : IRequestHandler<GetBadgesFromEmailQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToAssignHandler> _logger;

        public GetBadgesFromEmailHandler(NetBairesContext context,
            IMapper mapper,
            ILogger<GetBadgesFromEmailHandler> logger)
        {
            _context = context;
            this._mapper = mapper;
        }


        public async Task<IActionResult> Handle(GetBadgesFromEmailQuery request, CancellationToken cancellationToken)
        {
            var badges = _context.BadgeMembers.Where(x => x.Member.Email.ToUpper() == request.Email.ToUpper())
                .Select((x) => _mapper.Map(x.Badge, new BadgeDetailViewModel()));
            if (!badges.Any())
                return new StatusCodeResult(204);

            return new ObjectResult(badges) { StatusCode = 200 };
        }
    }
}