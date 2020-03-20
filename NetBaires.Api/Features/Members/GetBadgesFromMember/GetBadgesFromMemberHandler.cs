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

namespace NetBaires.Api.Features.Members.GetBadgesFromMember
{
    public class GetBadgesFromMemberHandler : IRequestHandler<GetBadgesFromMemberQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public GetBadgesFromMemberHandler(NetBairesContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(GetBadgesFromMemberQuery request, CancellationToken cancellationToken)
        {
            var badges = await _context.BadgeMembers.Include(x => x.Badge).Where(x =>
                    (request.Email != "" ? x.Member.Email.ToUpper() == request.Email.ToUpper() : true)
                    &&
                    (request.Id != null ? x.MemberId == request.Id : true))
                .Cacheable()
                .ProjectTo<BadgeMemberViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);

            if (!badges.Any())
                return HttpResponseCodeHelper.NotContent();

            return HttpResponseCodeHelper.Ok(badges);

        }
    }
}