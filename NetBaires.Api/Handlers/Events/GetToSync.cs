using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Handlers.Events.Models;
using NetBaires.Api.Models;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetToSyncHandler : IRequestHandler<GetToSyncHandler.GetToSync, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToSyncHandler> _logger;

        public GetToSyncHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IMapper mapper,
            IOptions<AssistanceOptions> assistanceOptions,
            ILogger<GetToSyncHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetToSync request, CancellationToken cancellationToken)
        {
            var eventToReturn = _context.Events.OrderByDescending(x => x.Id).Where(x => !x.Done)?
                .Select(x => new GetToAsyncResponseViewModel(x,
                    x.Attendees.Count(s => s.Status == EventMemberStatus.Attended),
                    x.Attendees.Count(s => s.Status == EventMemberStatus.DidNotAttend)))?.ToList();

            if (eventToReturn == null)
                return new StatusCodeResult(404);

            return new ObjectResult(eventToReturn) { StatusCode = 200 };

        }


        public class GetToSync : IRequest<IActionResult>
        {
         
        }

    }
}