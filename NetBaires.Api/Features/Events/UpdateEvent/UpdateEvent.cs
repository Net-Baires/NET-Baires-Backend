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
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public UpdateEventHandler(IMapper mapper,
            NetBairesContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await _context.Events.Include(x => x.Sponsors).FirstOrDefaultAsync(x => x.Id == request.Id);
            if (eventToUpdate == null)
                return new StatusCodeResult(402);

            _mapper.Map(request, eventToUpdate);

            if (request?.Live == true)
                eventToUpdate.SetLive();
            else if (request?.Live == false)
                eventToUpdate.SetUnLive();
            if (request?.GeneralAttended == true)
                eventToUpdate.EnableGeneralAttendace();
            else if (request?.GeneralAttended == false)
                eventToUpdate.DisableGeneralAttendace();

            
         


            _context.Entry(eventToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();


            return new ObjectResult(_mapper.Map(eventToUpdate, new EventDetail())) { StatusCode = 200 };
        }
    }
}