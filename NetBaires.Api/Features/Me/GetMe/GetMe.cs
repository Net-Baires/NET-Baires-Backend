using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Me.GetMe
{

    public class GetMeHandler : IRequestHandler<GetMeQuery, IActionResult>
    {
        private readonly ICurrentUser currentUser;
        private readonly IMapper _mapper;
        private readonly NetBairesContext _context;
        private readonly ILogger<GetMeHandler> _logger;

        public GetMeHandler(ICurrentUser currentUser,
            IMapper mapper,
            NetBairesContext context,
            ILogger<GetMeHandler> logger)
        {
            this.currentUser = currentUser;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var currentMemberId = currentUser.User.Id;
            var member = await _context.Members.Include(x=> x.Events).FirstOrDefaultAsync(x => x.Id == currentMemberId);

            var memberToResponse = _mapper.Map(member, new MemberDetailViewModel());

            return new ObjectResult(memberToResponse) { StatusCode = 200 };
        }
    }
}