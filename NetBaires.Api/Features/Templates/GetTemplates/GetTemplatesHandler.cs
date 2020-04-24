using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Templates.GetTemplates
{

    public class GetTemplatesHandler : IRequestHandler<GetTemplatesQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public GetTemplatesHandler(NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetTemplatesQuery request, CancellationToken cancellationToken)
        {
            var templates = await _context.Templates.Where(x => (request.Id == null || x.Id == request.Id))
                .ProjectTo<TemplateViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);
            if (templates == null)
                return new StatusCodeResult(204);

            if (request.Id != null)
                return HttpResponseCodeHelper.Ok(templates.First());
            return HttpResponseCodeHelper.Ok(templates);

        }
    }
}