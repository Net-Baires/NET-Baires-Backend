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

namespace NetBaires.Api.Features.Members.GetMemberDetail
{

    public class GetMemberDetailHandler : IRequestHandler<GetMemberDetailQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public GetMemberDetailHandler(
            NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(GetMemberDetailQuery request, CancellationToken cancellationToken)
        {
            var member = await _context.Members.Include(x=> x.Events).Cacheable()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (member == null)
                return HttpResponseCodeHelper.NotFound();

            return HttpResponseCodeHelper.Ok(_mapper.Map<MemberDetailViewModel>(member));

        }
    }
}