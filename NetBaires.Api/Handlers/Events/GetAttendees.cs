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
using NetBaires.Api.Handlers.Events.Models;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetAttendeesHandler : IRequestHandler<GetAttendeesHandler.GetAttendees, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public GetAttendeesHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            IOptions<AssistanceOptions> assistanceOptions,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetAttendees request, CancellationToken cancellationToken)
        {
            var attendees = await _context.EventMembers
                                        .Include(x => x.Member)
                                        .Where(x => x.EventId == request.Id)
                                        .ToListAsync();
            if (attendees == null || !attendees.Any())
                return new StatusCodeResult(204);

            return new ObjectResult(_mapper.Map(attendees, new List<GetAttendeesResponse>())) { StatusCode = 200 };
        }


        public class GetAttendees : IRequest<IActionResult>
        {
            public GetAttendees(int id)
            {
                Id = id;
            }

            public int Id { get; }

        }
        public class GetAttendeesResponse
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Picture { get; set; }
            public EventMemberStatus Status { get; set; }
        }
        public class GetAttendeesProfile : Profile
        {
            public GetAttendeesProfile()
            {
                CreateMap<EventMember, GetAttendeesResponse>()
                .ForMember(dest => dest, opt => opt.MapFrom(src => src.Member));

            }
        }

    }
}