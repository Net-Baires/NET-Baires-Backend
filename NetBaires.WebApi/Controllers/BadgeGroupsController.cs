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
using NetBaires.Api.Services.BadGr;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BadgeGroupsController : ControllerBase
    {
        private readonly NetBairesContext _context;
        private readonly IMediator _mediator;
        private readonly IBadGrServices _badGrServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<BadgesController> _logger;

        public BadgeGroupsController(IHttpClientFactory httpClientFactory,
            NetBairesContext context,
            IMediator mediator,
            IBadGrServices badGrServices,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<BadgesController> logger)
        {
            _context = context;
            _mediator = mediator;

            _badGrServices = badGrServices;
            _mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _logger = logger;
            httpClientFactory.CreateClient();
        }


        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public IActionResult Get()
        {
            var badges = _context.Badges.AsNoTracking();

            return Ok(badges);
        }
        [HttpGet("{id}/badges")]
        [SwaggerOperation(Summary = "Retorna todos los badges asignados al grupo ")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public IActionResult GetBadgesFromGroup(int id)
        {
            var badges = _context.Badges.AsNoTracking();

            return Ok(badges);
        }

        [HttpGet("{groupId}")]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public async Task<IActionResult> Get(int groupId)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == groupId);
            if (badge == null)
                return NotFound();

            return Ok(_mapper.Map(badge, new BadgeDetailViewModel()));
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Post([FromForm]NewBadgeCommand badge)
        {

            foreach (var item in httpContextAccessor.HttpContext.Request.Form.Files)
            {
                var a = item;
            }
            return await _mediator.Send(badge);
        }

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Put([FromRoute]int id, UpdateBadgeCommand badge)
        {
            badge.Id = id;

            return await _mediator.Send(badge);
        }
        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete([FromRoute]int id) =>
            await _mediator.Send(new DeleteBadgeCommand(id));
    }
}
