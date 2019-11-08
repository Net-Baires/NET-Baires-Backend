using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Sponsors
{

    public class GetSponsorsHandler : IRequestHandler<GetSponsorsHandler.GetSponsors, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSponsorsHandler> _logger;

        public GetSponsorsHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IMapper mapper,
            ILogger<GetSponsorsHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetSponsors request, CancellationToken cancellationToken)
        {
            var sponsors = await _context.Sponsors.ToListAsync();

            if (!sponsors.Any())
                return new StatusCodeResult(204);

            var sponsorsToReturn = _mapper.Map(sponsors, new List<GetSponsorsResponse>());


            return new ObjectResult(sponsorsToReturn) { StatusCode = 200 };

        }

        public class GetSponsors : IRequest<IActionResult>
        {
        }
        public class GetSponsorsResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string SiteUrl { get; set; }
            public string Description { get; set; }
            public string LogoUrl { get; set; }
        }
        public class GetSponsorsProfile : Profile
        {
            public GetSponsorsProfile()
            {
                CreateMap<Sponsor, GetSponsorsResponse>();
            }
        }

    }
}