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

    public class UpdateAttendeeHandler : IRequestHandler<UpdateAttendeeHandler.UpdateAttendee, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public UpdateAttendeeHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            IOptions<AssistanceOptions> assistanceOptions,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(UpdateAttendee request, CancellationToken cancellationToken)
        {
            var attendee = await _context.Members
                                        .FirstOrDefaultAsync(x => x.Events.Any(s => s.EventId == request.IdEvent && s.MemberId == request.IdMember));
            if (attendee == null)
                return new StatusCodeResult(404);

            var newAttend = new EventMember(request.IdMember, request.IdEvent);


            switch (request.Status)
            {
                case EventMemberStatus.Attended:
                    newAttend.Attend();
                    break;
                case EventMemberStatus.DidNotAttend:
                    newAttend.NoAttend();
                    break;
                case EventMemberStatus.NotifyAbsence:
                    newAttend.NotifyAbsence();
                    break;
                default:
                    break;
            }
            await _context.SaveChangesAsync();
            return new StatusCodeResult(204);
        }


        public class UpdateAttendee : IRequest<IActionResult>
        {
            public UpdateAttendee(int idEvent, int idMember, EventMemberStatus status)
            {
                Status = status;
                IdEvent = idEvent;
                IdMember = idMember;
            }
            public EventMemberStatus Status { get; set; }
            public int IdEvent { get; }
            public int IdMember { get; }

        }
        public class UpdateAttendeeResponse
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Picture { get; set; }
        }
        public class UpdateAttendeeProfile : Profile
        {
            public UpdateAttendeeProfile()
            {
                CreateMap<Member, UpdateAttendeeResponse>();
            }
        }

    }
}