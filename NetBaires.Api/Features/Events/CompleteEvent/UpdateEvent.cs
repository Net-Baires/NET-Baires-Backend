using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.CompleteEvent
{

    public class CompleteEventHandler : IRequestHandler<CompleteEventCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ILogger<CompleteEventHandler> _logger;

        public CompleteEventHandler(
            NetBairesContext context,
            ILogger<CompleteEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(CompleteEventCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _context.Events.Include(x => x.Attendees).FirstOrDefaultAsync(x => x.Id == request.Id);
            if (eventToUpdate == null)
                return HttpResponseCodeHelper.NotFound();

            eventToUpdate.Complete();

            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
}