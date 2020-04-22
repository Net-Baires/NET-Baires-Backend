using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.EventInformation.GetEventInformation
{
    public class GetEventInformationHandler : IRequestHandler<GetEventInformationQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEventInformationHandler> _logger;

        public GetEventInformationHandler(NetBairesContext context,
            IMapper mapper,
            ILogger<GetEventInformationHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(GetEventInformationQuery request, CancellationToken cancellationToken)
        {
            var materials = await _context.EventInformation.Where(x => x.Event.Id == request.EventId
                                        &&
                                        (request.Visible == null || x.Visible == request.Visible))
                .ProjectTo<EventInformationViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (!materials.Any())
                return HttpResponseCodeHelper.NotContent();
            return HttpResponseCodeHelper.Ok(materials);
        }
    }
}