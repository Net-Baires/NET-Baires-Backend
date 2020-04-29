using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Materials.AddMaterial;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.EventInformation.AddEventInformation
{

    public class AddEventInformationHandler : IRequestHandler<AddEventInformationCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ILogger<AddMaterialHandler> _logger;

        public AddEventInformationHandler(NetBairesContext context,
            ILogger<AddMaterialHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(AddEventInformationCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events.Include(x => x.Attendees)
                                              .FirstOrDefaultAsync(x => x.Id == request.EventId);
            if (@event == null)
                return HttpResponseCodeHelper.Error("El evento indicado no existe");
            @event.AddInformation(request.Title, request.Description, request.Visible);

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
}