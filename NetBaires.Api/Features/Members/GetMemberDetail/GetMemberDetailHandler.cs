using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EFSecondLevelCache.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Members.GetMemberDetail
{

    public class GetMemberDetailHandler : IRequestHandler<GetMemberDetailQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public GetMemberDetailHandler(
            NetBairesContext context,
            ICurrentUser currentUser,
            IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(GetMemberDetailQuery request, CancellationToken cancellationToken)
        {
            var member = await _context.Members.Include(x => x.Events).Cacheable()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (member == null)
                return HttpResponseCodeHelper.NotFound();

            var memberToReturn = _mapper.Map<MemberDetailViewModel>(member);
            var list = await _context.FollowingMembers.Include(x => x.Member).Include(x => x.Following).ToListAsync();
            if (_currentUser.IsLoggued)
            {
                memberToReturn.Following = await _context.FollowingMembers.AnyAsync(x => x.Member.Id == _currentUser.User.Id
                                                                                         &&
                                                                                         x.Following.Id == request.Id, cancellationToken: cancellationToken);

                memberToReturn.Followed = await _context.FollowingMembers.AnyAsync(x => x.Member.Id == request.Id
                                                                                         &&
                                                                                         x.Following.Id == _currentUser.User.Id, cancellationToken: cancellationToken);
            }
            return HttpResponseCodeHelper.Ok(memberToReturn);

        }
    }
}