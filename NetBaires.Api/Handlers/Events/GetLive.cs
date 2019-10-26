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
using NetBaires.Api.Handlers.Events.Models;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetLiveHandler : IRequestHandler<GetLiveHandler.GetLive, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public GetLiveHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            IOptions<AttendanceOptions> assistanceOptions,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetLive request, CancellationToken cancellationToken)
        {
            var eventResponse = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (!eventResponse.Live)
                return new StatusCodeResult(404);

            return new ObjectResult(eventResponse) { StatusCode = 200 };
        }


        public class GetLive : IRequest<IActionResult>
        {
            public GetLive(int id)
            {
                Id = id;
            }

            public int Id { get; }

        }
        public class GetLiveProfile : Profile
        {
            public GetLiveProfile()
            {
                CreateMap<GetLive, Event>().ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null)); ;
            }
        }

    }
}