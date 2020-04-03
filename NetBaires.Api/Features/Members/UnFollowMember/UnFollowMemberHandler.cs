using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Members.UnFollowMember
{

    public class UnFollowMemberHandler : IRequestHandler<UnFollowMemberCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public UnFollowMemberHandler(
            NetBairesContext context,
            ICurrentUser currentUser,
            IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(UnFollowMemberCommand request, CancellationToken cancellationToken)
        {
            var me = await _context.Members.FirstOrDefaultAsync(x => x.Id == _currentUser.User.Id);
            var following = await _context.FollowedMembers.FirstOrDefaultAsync(x => x.Member.Id == _currentUser.User.Id
                                                                                                     &&
                                                                                                     x.Followed.Id == request.MemberId);
            if (following == null)
                return HttpResponseCodeHelper.NotFound();
            me.UnFollow(following);
            await _context.SaveChangesAsync();

            return HttpResponseCodeHelper.NotContent();
        }
    }
}