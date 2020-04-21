using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Events.AddAttendee
{

    public class AddAttendeeHandler : IRequestHandler<AddAttendeeCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ILogger<AddAttendeeHandler> _logger;

        public AddAttendeeHandler(NetBairesContext context,
            ILogger<AddAttendeeHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(AddAttendeeCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events.Include(x => x.Attendees)
                                              .FirstOrDefaultAsync(x => x.Id == request.IdEvent);
            if (@event == null)
                return HttpResponseCodeHelper.Error("El evento indicado no existe");

            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.IdMember);
            if (member == null)
                return HttpResponseCodeHelper.Error("El miembro indicado no existe");

            @event.AddAttendance(member, AttendanceRegisterType.CurrentEvent);

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
    public class AddAttendeeCommand : IRequest<IActionResult>
    {
        public int IdEvent { get; set; }
        public int IdMember { get; set; }
    }
}