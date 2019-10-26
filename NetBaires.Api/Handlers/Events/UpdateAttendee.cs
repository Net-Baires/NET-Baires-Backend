

using System.Text.Json.Serialization;
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

    public class UpdateAttendeeHandler : IRequestHandler<UpdateAttendeeHandler.UpdateAttendee, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<UpdateEventHandler> _logger;

        public UpdateAttendeeHandler(IMapper mapper,
            NetBairesContext context,
            ILogger<UpdateEventHandler> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(UpdateAttendee request, CancellationToken cancellationToken)
        {
            var attendee = await _context.EventMembers
                                        .FirstOrDefaultAsync(x => x.EventId == request.EventId && x.MemberId == request.MemberId);
            if (attendee == null)
                return new StatusCodeResult(404);


            _mapper.Map(request, attendee);
            await _context.SaveChangesAsync();
            return new StatusCodeResult(204);
        }


        public class UpdateAttendee : IRequest<IActionResult>
        {
            public string Status { get; set; }
            public bool Organizer { get; set; }
            public bool Speaker { get; set; }
            [JsonIgnore]
            public int EventId { get; internal set; }
            [JsonIgnore]
            public int MemberId { get; internal set; }
            public bool Attended { get; set; }
            public bool NotifiedAbsence { get; set; }
            public bool DoNotKnow { get; set; }
        }
        public class UpdateAttendeeResponse
        {

        }
        public class UpdateAttendeeProfile : Profile
        {
            public UpdateAttendeeProfile()
            {
                CreateMap<UpdateAttendee, EventMember>();
            }
        }

    }
}