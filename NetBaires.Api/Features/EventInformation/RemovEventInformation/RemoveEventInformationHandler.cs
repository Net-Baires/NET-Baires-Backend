using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.EventInformation.RemovEventInformation
{
    public class RemoveEventInformationHandler : IRequestHandler<RemoveEventInformationCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ILogger<RemoveEventInformationHandler> _logger;

        public RemoveEventInformationHandler(NetBairesContext context,
            ILogger<RemoveEventInformationHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(RemoveEventInformationCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events.Include(x => x.Attendees)
                .FirstOrDefaultAsync(x => x.Id == request.EventId);
            if (@event == null)
                return HttpResponseCodeHelper.Error("El evento indicado no existe");


            var eventInformation = await _context.EventInformation.FirstOrDefaultAsync(x => x.Id == request.EventInformationId);

            if (eventInformation == null)
                return HttpResponseCodeHelper.Error("La información que intenga eliminar, no existe");

            @event.RemoveInformation(eventInformation);

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
}