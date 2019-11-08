using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Badges;
using NetBaires.Api.Features.Badges.DeleteBadge;
using NetBaires.Api.Features.Badges.Models;
using NetBaires.Api.Features.Badges.NewBadge;
using NetBaires.Api.Features.Badges.UpdateBadge;
using NetBaires.Api.Handlers.Events;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.BadgeGroups
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BadgeGroupsController : ControllerBase
    {
        private readonly NetBairesContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<BadgesController> _logger;

        public BadgeGroupsController(IHttpClientFactory httpClientFactory,
            NetBairesContext context,
            IMediator mediator,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<BadgesController> logger)
        {
            _context = context;
            _mediator = mediator;

            _mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _logger = logger;
            httpClientFactory.CreateClient();
        }


        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public async Task<IActionResult> GetAsync() =>
            await _mediator.Send(new GetBadgeGroupsQuery());

        [HttpGet("{BadgeGroupId}")]
        [SwaggerOperation(Summary = "Retorna el detalle de un badge de NET-Baires")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public async Task<IActionResult> Get([FromRoute] GetBadgeGroupQuery query) =>
            await _mediator.Send(query);

        [HttpPost]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Post([FromBody]NewBadgeGroupCommand badge) =>
                   await _mediator.Send(badge);

        [HttpPost("{badgeGroupId}/badges/{badgeId}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Post([FromRoute]AssignBadgeToBadgeGroupCommand badge) =>
           await _mediator.Send(badge);

    }
}
