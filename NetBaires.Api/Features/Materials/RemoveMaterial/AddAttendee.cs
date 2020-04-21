using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Materials.RemoveMaterial
{

    public class RemoveMaterialHandler : IRequestHandler<RemoveMaterialCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ILogger<RemoveMaterialHandler> _logger;

        public RemoveMaterialHandler(NetBairesContext context,
            ILogger<RemoveMaterialHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(RemoveMaterialCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events.Include(x => x.Attendees)
                                              .FirstOrDefaultAsync(x => x.Id == request.EventId);
            if (@event == null)
                return HttpResponseCodeHelper.Error("El evento indicado no existe");


            var material = await _context.Materials.FirstOrDefaultAsync(x => x.Id == request.MaterialId);

            if (material == null)
                return HttpResponseCodeHelper.Error("El Material indicado no existe");

            @event.RemoveMaterial(material);

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
    public class RemoveMaterialCommand : IRequest<IActionResult>
    {
        public int EventId { get; set; }
        public int MaterialId { get; set; }

        public RemoveMaterialCommand(int eventId, int materialId)
        {
            EventId = eventId;
            MaterialId = materialId;
        }
    }
}