using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Handlers.Events.Models;
using NetBaires.Api.Helpers;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class CompleteEventHandler : IRequestHandler<CompleteEvent, IActionResult>
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


        public async Task<IActionResult> Handle(CompleteEvent request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _context.Events.Include(x => x.Attendees).FirstOrDefaultAsync(x => x.Id == request.Id);
            if (eventToUpdate == null)
                return HttpResponseCodeHelper.NotFound();

            eventToUpdate.Complete();

            await _context.SaveChangesAsync();


            return HttpResponseCodeHelper.NotContent();
        }
    }
    public class CompleteEvent : IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
}