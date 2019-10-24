using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Handlers.Badges
{
    public class UpdateBadgeHandler : IRequestHandler<UpdateBadgeHandler.UpdateBadge, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetToAssignHandler> _logger;

        public UpdateBadgeHandler(ICurrentUser currentUser,
            NetBairesContext context,
            IMapper mapper,
            IOptions<AssistanceOptions> assistanceOptions,
            ILogger<GetToAssignHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<IActionResult> Handle(UpdateBadge request, CancellationToken cancellationToken)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (badge == null)
                return new StatusCodeResult(404)
                    ;
            _mapper.Map(request, badge);

            await _context.SaveChangesAsync();

            return new ObjectResult(request) { StatusCode = 200 };

        }

        private List<GetToAssignHandler.BadgeAssignResponse> ReduceBadge(Badge item, List<GetToAssignHandler.BadgeAssignResponse> accum, bool assigned)
        {
            var returnValue = _mapper.Map(item, new GetToAssignHandler.BadgeAssignResponse());
            returnValue.Assigned = assigned;
            accum.Add(returnValue);
            return accum;
        }

        public class UpdateBadge : IRequest<IActionResult>
        {
            public int Id { get; set; }
            public string BadgeUrl { get; set; }
            public DateTime Created { get; set; }
            public string BadgeImageUrl { get; set; }
            public string IssuerUrl { get; set; }
            public string Image { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class UpdateBadgeProfile : Profile
        {
            public UpdateBadgeProfile()
            {
                CreateMap<UpdateBadge, Badge>().ForAllMembers(
                    opt => opt.Condition((src, dest, sourceMember) => sourceMember != null)); ; ;
            }
        }
    }
}