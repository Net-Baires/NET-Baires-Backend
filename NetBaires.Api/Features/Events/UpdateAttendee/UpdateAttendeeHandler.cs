using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Events.ViewModels;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class UpdateAttendeeHandler : IRequestHandler<UpdateAttendeeCommand, IActionResult>
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


        public async Task<IActionResult> Handle(UpdateAttendeeCommand request, CancellationToken cancellationToken)
        {
            var attendee = await _context.Attendances
                                         .Include(x => x.Member)
                                        .FirstOrDefaultAsync(x => x.EventId == request.EventId
                                                                  &&
                                                                  x.MemberId == request.MemberId);
            if (attendee == null)
                return HttpResponseCodeHelper.NotFound();

            if (request?.Speaker == true)
                attendee.SetSpeaker();
            else if (request?.Speaker == false)
                attendee.RemoveSpeaker();

            if (request?.Attended == true)
                attendee.Attend();
            else if (request?.Attended == false)
                attendee.NoAttend();

            if (request?.NotifiedAbsence == true)
                attendee.NotifyAbsence();
            else if (request?.NotifiedAbsence == false)
                attendee.NoAttend();

            if (request?.Organizer == true)
                attendee.SetOrganizer();
            else if (request?.Organizer == false)
                attendee.RemoveOrganizer();

            await _context.SaveChangesAsync();
            return HttpResponseCodeHelper.Ok(_mapper.Map<AttendantViewModel>(attendee));
        }
    }
}