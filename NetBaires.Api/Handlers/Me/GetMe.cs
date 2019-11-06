using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Events
{

    public class GetMeHandler : IRequestHandler<GetMeHandler.GetMe, IActionResult>
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


        public async Task<IActionResult> Handle(GetMe request, CancellationToken cancellationToken)
        {
            var currentMemberId = currentUser.User.Id;
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == currentMemberId);

            var memberToResponse = _mapper.Map(member, new GetMeResponse());

            return new ObjectResult(memberToResponse) { StatusCode = 200 };
        }


        public class GetMe : IRequest<IActionResult>
        {

        }
        public class GetMeResponse
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }
            public string WorkPosition { get; set; }
            public string Twitter { get; set; }
            public string Instagram { get; set; }
            public string Linkedin { get; set; }
            public string Github { get; set; }
            public string Biography { get; set; }
            public string Picture { get; set; }

        }
        public class GetMeProfile : Profile
        {
            public GetMeProfile()
            {
                CreateMap<Member, GetMeResponse>();
            }
        }

    }
}