using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Badges.NewBadge;
using NetBaires.Data;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.BadgeGroups.NewBadgeGroups
{
    public class NewBadgeGroupHandler : IRequestHandler<NewBadgeGroupCommand, IActionResult>
    {
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<NewBadgeHandler> logger;

        public NewBadgeGroupHandler(NetBairesContext context,
            IMapper mapper,
            ILogger<NewBadgeHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            this.logger = logger;
        }


        public async Task<IActionResult> Handle(NewBadgeGroupCommand request, CancellationToken cancellationToken)
        {
            await _context.BadgeGroups.AddAsync(_mapper.Map(request, new BadgeGroup()));
            await _context.SaveChangesAsync();
            return new StatusCodeResult(204);
        }
    }
}