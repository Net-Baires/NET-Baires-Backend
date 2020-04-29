using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EFSecondLevelCache.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Sponsors.GetSponsors
{

    public class GetSponsorsHandler : IRequestHandler<GetSponsorsQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public GetSponsorsHandler(NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetSponsorsQuery request, CancellationToken cancellationToken)
        {
            var sponsor = await _context.Sponsors.Where(x => (request.Id != null ? x.Id == request.Id : true)).Cacheable().ToListAsync(cancellationToken: cancellationToken);

            if (sponsor == null)
                return new StatusCodeResult(204);

            var sponsorsToReturn = _mapper.Map<List<SponsorDetailViewModel>>(sponsor);
            if (request.Id != null)
                return HttpResponseCodeHelper.Ok(sponsorsToReturn.First());
            return HttpResponseCodeHelper.Ok(sponsorsToReturn);

        }
    }
}