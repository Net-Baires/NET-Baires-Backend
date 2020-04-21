using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Helpers;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Materials.GetMaterials
{

    public class GetMaterialsHandler : IRequestHandler<GetMaterialsQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMaterialsHandler> _logger;

        public GetMaterialsHandler(NetBairesContext context,
            IMapper mapper,
            ILogger<GetMaterialsHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
        {
            var materials = await _context.Materials.Where(x => x.Event.Id == request.EventId)
                .ProjectTo<MaterialViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (!materials.Any())
                return HttpResponseCodeHelper.NotContent();
            return HttpResponseCodeHelper.Ok(materials);
        }
    }
    public class GetMaterialsQuery : IRequest<IActionResult>
    {
        public GetMaterialsQuery(int eventId)
        {
            EventId = eventId;
        }

        public int EventId { get;  }
    }

    public class MaterialViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public class MaterialMaterialViewModelProfile : Profile
        {
            public MaterialMaterialViewModelProfile()
            {
                CreateMap<Material, MaterialViewModel>();
            }
        }
    }
}