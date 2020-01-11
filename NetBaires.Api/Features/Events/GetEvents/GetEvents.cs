using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Events.GetEvents
{

    public class GetEventsHandler : IRequestHandler<GetEventsQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public GetEventsHandler(IMapper mapper,
            NetBairesContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {

            var eventToReturn = await _context.Events.Include(x => x.Attendees)
                                                     .Include(x => x.Sponsors)
                                                     .Where(x => (request.Done != null ? x.Done == request.Done : true)
                                                            &&
                                                            (request.Live != null ? x.Live == request.Live : true)
                                                            &&
                                                            (request.Id != null ? x.Id == request.Id : true))
                                               .ProjectTo<EventDetailViewModel>(_mapper.ConfigurationProvider)
                                               .ToListAsync();
            if (!eventToReturn.Any())
                return HttpResponseCodeHelper.NotContent();

            if (request.Id != null)
                return HttpResponseCodeHelper.Ok(eventToReturn.First());
            return HttpResponseCodeHelper.Ok(eventToReturn);
        }
    }

}