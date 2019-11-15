using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Organizers
{
    public class GetOrganizersHandler : IRequestHandler<GetOrganizersQuery, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;

        public GetOrganizersHandler(IMapper mapper,
            NetBairesContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IActionResult> Handle(GetOrganizersQuery request, CancellationToken cancellationToken)
        {
            var eventResponse = await _context.Members.Where(x => x.Organized).ToListAsync();

            if (eventResponse == null)
                return HttpResponseCodeHelper.NotContent();

            return HttpResponseCodeHelper.Ok(_mapper.Map<List<MemberDetailViewModel>>(eventResponse));
        }
    }
}