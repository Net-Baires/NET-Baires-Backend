using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Events.AddMaterial
{

    public class AddMaterialHandler : IRequestHandler<AddMaterialCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ILogger<AddMaterialHandler> _logger;

        public AddMaterialHandler(NetBairesContext context,
            ILogger<AddMaterialHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(AddMaterialCommand request, CancellationToken cancellationToken)
        {
            var @event = await _context.Events.Include(x => x.Attendees)
                                              .FirstOrDefaultAsync(x => x.Id == request.EventId);
            if (@event == null)
                return HttpResponseCodeHelper.Error("El evento indicado no existe");
            @event.AddMaterial(request.Title, request.Link);

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
    public class AddMaterialCommand : IRequest<IActionResult>
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
    }
}