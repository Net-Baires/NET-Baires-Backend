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
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class AddAttendeeHandler : IRequestHandler<AddAttendeeHandler.AddAttendee, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public AddAttendeeHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            IOptions<AttendanceOptions> assistanceOptions,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(AddAttendee request, CancellationToken cancellationToken)
        {
            var attendees = await _context.Members
                                        .FirstOrDefaultAsync(x => x.Events.Any(s => s.EventId == request.IdEvent && s.MemberId == request.IdMember));
            if (attendees != null)
                return new StatusCodeResult(400);

            var newAttend = new Attendance(request.IdMember, request.IdEvent);
            await _context.Attendances.AddAsync(newAttend);
            await _context.SaveChangesAsync();
            return new ObjectResult(_mapper.Map(attendees, new List<AddAttendeeResponse>())) { StatusCode = 200 };
        }


        public class AddAttendee : IRequest<IActionResult>
        {
            public AddAttendee(int idEvent, int idMember)
            {
                IdEvent = idEvent;
                IdMember = idMember;
            }

            public int IdEvent { get; }
            public int IdMember { get; }

        }
        public class AddAttendeeResponse
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Picture { get; set; }
        }
        public class AddAttendeeProfile : Profile
        {
            public AddAttendeeProfile()
            {
                CreateMap<Member, AddAttendeeResponse>();
            }
        }

    }
}