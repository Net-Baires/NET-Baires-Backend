using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EFSecondLevelCache.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using AutoMapper.QueryableExtensions;
namespace NetBaires.Api.Features.Me.GetMe
{

    public class GetMeHandler : IRequestHandler<GetMeQuery, IActionResult>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<GetMeHandler> _logger;

        public GetMeHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            ILogger<GetMeHandler> logger)
        {
            this._currentUser = currentUser;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var currentMemberId = _currentUser.User.Id;
            var member = await _context.Members.Include(s => s.Events).ProjectTo<MemberDetailViewModel>(_mapper.ConfigurationProvider)
                .Cacheable()

                                               .FirstOrDefaultAsync(x => x.Id == currentMemberId, cancellationToken);

            var memberToResponse = _mapper.Map(member, new MemberDetailViewModel());

            memberToResponse.FollowedMembers = await _context.FollowingMembers.Cacheable()
                                                        .Where(x => x.FollowingId == currentMemberId)
                                                        .Select(x => x.Member.Id)
                                                        .ToListAsync(cancellationToken);

            return HttpResponseCodeHelper.Ok(memberToResponse);
        }
    }
}