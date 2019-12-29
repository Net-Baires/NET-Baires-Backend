using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.BadgeGroups.ViewModels;
using NetBaires.Api.Features.Events.UpdateEvent;
using NetBaires.Data;

namespace NetBaires.Api.Features.BadgeGroups.GetBadgeGroup
{

    public class GetBadgeGroupHandler : IRequestHandler<GetBadgeGroupQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public GetBadgeGroupHandler(IMapper mapper,
                               NetBairesContext context,
                               ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetBadgeGroupQuery request, CancellationToken cancellationToken)
        {

            var badgeGroup = await _context.BadgeGroups.Include(x => x.Badges).FirstOrDefaultAsync(x => x.Id == request.BadgeGroupId);
            if (badgeGroup == null)
                return new StatusCodeResult(204);
            var returnValue = new BadgeGroupDetailViewModel(badgeGroup, badgeGroup.Badges.Count());

            return new ObjectResult(returnValue) { StatusCode = 200 };
        }

    }
    public class GetBadgeGroupQuery : IRequest<IActionResult>
    {
        public int BadgeGroupId { get; set; }
    }

}