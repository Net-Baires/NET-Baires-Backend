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

namespace NetBaires.Api.Features.Members.GetBadgeFromMember
{
    public class GetBadgeFromMemberHandler : IRequestHandler<GetBadgeFromMemberQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public GetBadgeFromMemberHandler(NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(GetBadgeFromMemberQuery request, CancellationToken cancellationToken)
        {
            var badge = await _context.BadgeMembers.Include(x => x.Badge)
                .Where(x=> x.MemberId ==request.Id
                           &&
                           x.BadgeId == request.BadgeId)
                .ProjectTo<BadgeMemberViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (badge == null)
                return HttpResponseCodeHelper.NotFound("No se encuentra el badge solicitado para el miembro indicado.");

            return HttpResponseCodeHelper.Ok(badge);
        }
    }
}