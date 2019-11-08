using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetBadgeGroupstHandler : IRequestHandler<GetBadgeGroupsQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public GetBadgeGroupstHandler(IMapper mapper,
                               NetBairesContext context,
                               ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetBadgeGroupsQuery request, CancellationToken cancellationToken)
        {

            var eventToReturn = _context.BadgeGroups.Include(x=> x.Badges).ToList()?.Select(x => new BadgeGroupDetailViewModel(x, x.Badges.Count()))?.ToList();

            if (eventToReturn == null)
                return new StatusCodeResult(204);

            return new ObjectResult(eventToReturn) { StatusCode = 200 };
        }

    }
    public class GetBadgeGroupsQuery : IRequest<IActionResult>
    {
    }
   
 
}