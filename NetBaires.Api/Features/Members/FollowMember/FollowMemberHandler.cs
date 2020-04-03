using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Members.FollowMember
{

    public class FollowMemberHandler : IRequestHandler<FollowMemberCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public FollowMemberHandler(
            NetBairesContext context,
            ICurrentUser currentUser,
            IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(FollowMemberCommand request, CancellationToken cancellationToken)
        {
            var me = await _context.Members.FirstOrDefaultAsync(x => x.Id == _currentUser.User.Id, cancellationToken: cancellationToken);
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.MemberId, cancellationToken: cancellationToken);
            if (member == null)
                return HttpResponseCodeHelper.NotFound();
            me.Follow(member);
            await _context.SaveChangesAsync(cancellationToken);

            return HttpResponseCodeHelper.NotContent();
        }
    }
}