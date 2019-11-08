using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Sponsors
{

    public class GetSponsorHandler : IRequestHandler<GetSponsorHandler.GetSponsor, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSponsorHandler> _logger;

        public GetSponsorHandler(NetBairesContext context,
            IMapper mapper,
            ILogger<GetSponsorHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetSponsor request, CancellationToken cancellationToken)
        {
            var sponsor = await _context.Sponsors.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (sponsor == null)
                return new StatusCodeResult(204);

            return new ObjectResult(_mapper.Map(sponsor, new GetSponsorResponse())) { StatusCode = 200 };

        }

        public class GetSponsor : IRequest<IActionResult>
        {
            public int Id { get; set; }
            public GetSponsor(int id)
            {
                Id = id;
            }
        }
        public class GetSponsorResponse
        {
            public int Id { get; set; }

            public string Name { get; set; }
            public string SiteUrl { get; set; }
            public string Description { get; set; }
            public string LogoUrl { get; set; }
        }
        public class GetSponsorProfile : Profile
        {
            public GetSponsorProfile()
            {
                CreateMap<Sponsor, GetSponsorResponse>();
            }
        }

    }
}