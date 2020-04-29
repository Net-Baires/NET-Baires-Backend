using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.EventInformation.GetEventInformation;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.EventInformation.UpdateEventInformation
{

    public class UpdateEventInformationHandler : IRequestHandler<UpdateEventInformationCommand, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public UpdateEventInformationHandler(IMapper mapper,
            NetBairesContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(UpdateEventInformationCommand request, CancellationToken cancellationToken)
        {
            var eventInformationToUpdate = await _context.EventInformation.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (eventInformationToUpdate == null)
                return new StatusCodeResult(402);

            _mapper.Map(request, eventInformationToUpdate);
            
            _context.Entry(eventInformationToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();


            return HttpResponseCodeHelper.Ok(_mapper.Map<EventInformationViewModel>(eventInformationToUpdate));
        }
    }
}