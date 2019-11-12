using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Members.ViewModels;
using NetBaires.Api.Helpers;
using NetBaires.Data;

namespace NetBaires.Api.Features.Badges.GetBadge
{

    public class GetMemberDetailHandler : IRequestHandler<GetMemberDetailQuery, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBadeHandler> _logger;

        public GetMemberDetailHandler(
            NetBairesContext context,
            IMapper mapper,
            ILogger<GetBadeHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(GetMemberDetailQuery request, CancellationToken cancellationToken)
        {
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (member != null)
                return HttpResponseCodeHelper.Ok(_mapper.Map(member, new MemberDetailViewModel()));

            return HttpResponseCodeHelper.NotFound();

        }
    }
}