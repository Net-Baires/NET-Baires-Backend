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
using NetBaires.Api.Handlers.Events;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Organizers
{

    public class GetOrganizersHandler : IRequestHandler<GetOrganizersHandler.GetOrganizers, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public GetOrganizersHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            IOptions<AttendanceOptions> assistanceOptions,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetOrganizers request, CancellationToken cancellationToken)
        {
            var eventResponse = await _context.Members.Where(x => x.Organized).ToListAsync();


            return new ObjectResult(eventResponse) { StatusCode = 200 };
        }


        public class GetOrganizers : IRequest<IActionResult>
        {


        }
        public class GetOrganizersProfile : Profile
        {
            public GetOrganizersProfile()
            {
                CreateMap<GetOrganizers, Event>().ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null)); ;
            }
        }

    }
}