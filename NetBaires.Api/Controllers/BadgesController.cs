using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Handlers.Badges;
using NetBaires.Api.Handlers.Badges.Models;
using NetBaires.Api.Models;
using NetBaires.Api.Services.BadGr;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BadgesController : ControllerBase
    {
        private readonly NetBairesContext _context;
        private readonly IMediator _mediator;
        private readonly IBadGrServices _badGrServices;
        private readonly IMapper _mapper;
        private readonly ILogger<BadgesController> _logger;

        public BadgesController(IHttpClientFactory httpClientFactory,
            NetBairesContext context,
            IMediator mediator,
            IBadGrServices badGrServices,
            IMapper mapper,
            ILogger<BadgesController> logger)
        {
            _context = context;
            _mediator = mediator;
            _badGrServices = badGrServices;
            _mapper = mapper;
            _logger = logger;
            httpClientFactory.CreateClient();
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Retorna todos los badges disponibles de NET-Baires")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public IActionResult Get()
        {
            var badges = _context.Badges.AsNoTracking();

            return Ok(badges);
        }

        [HttpGet("{badgeId}")]
        [SwaggerOperation(Summary = "Retorna todos los badges disponibles de NET-Baires")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public async Task<IActionResult> Get(int badgeId)
        {
            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == badgeId);
            if (badge == null)
                return NotFound();

            return Ok(_mapper.Map(badge, new BadgeDetailViewModel()));
        }

        [HttpGet("ToAssign")]
        [SwaggerOperation(Summary = "Retorna todos los badges disponibles de NET-Baires")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public async Task<IActionResult> GetToAssign([FromQuery] int memberId) =>
            await _mediator.Send(new GetToAssignHandler.GetToAssign(memberId));

        [HttpGet("{badgeId}/Members")]
        [SwaggerOperation(Summary = "Retorna de la lista de usuario que recibieron el Badge")]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> GetMembersInBadge(int badgeId)
        {
            var users = _context.Members.Where(x => x.Badges.Any(s => s.BadgeId == badgeId)).AsNoTracking();
            return Ok(users);
        }

        [HttpPost("{badgeId}/Member/{memberId}")]
        [SwaggerOperation(Summary = "Premia a un miembro con un Badge")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> AssignMembersInBadge([FromRoute]int badgeId, [FromRoute]int memberId)
        {
            var membersAlreadyHasTheBadge =
                _context.BadgeMembers.Any(x => x.BadgeId == badgeId && x.MemberId == memberId);
            if (membersAlreadyHasTheBadge)
                return BadRequest("El miembro que esta intentando asignar ya tiene ese Badge");

            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == badgeId);
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == memberId);
            var response = await _badGrServices.CreateAssertion(badge.BadgeId, member.Email);

            if (response.Status.Success)
            {
                _context.BadgeMembers.Add(new BadgeMember
                {
                    BadgeId = badge.Id,
                    MemberId = member.Id,
                    BadgeUrl = response.Result.First().OpenBadgeId.AbsoluteUri
                });
                await _context.SaveChangesAsync();
            }

            return Ok(member);
        }
        [HttpDelete("{badgeId}/Member/{memberId}")]
        [SwaggerOperation(Summary = "Premia a un miembro con un Badge")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> RemoveMembersInBadge([FromRoute]int badgeId, [FromRoute]int memberId)
        {
            var membersAlreadyHasTheBadge =
                _context.BadgeMembers.Any(x => x.BadgeId == badgeId && x.MemberId == memberId);
            if (membersAlreadyHasTheBadge)
                return BadRequest("El miembro que esta intentando asignar ya tiene ese Badge");

            var badge = await _context.Badges.FirstOrDefaultAsync(x => x.Id == badgeId);
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == memberId);
            var response = await _badGrServices.CreateAssertion(badge.BadgeId, member.Email);

            if (response.Status.Success)
            {
                _context.BadgeMembers.Add(new BadgeMember
                {
                    BadgeId = badge.Id,
                    MemberId = member.Id,
                    BadgeUrl = response.Result.First().OpenBadgeId.AbsoluteUri
                });
                await _context.SaveChangesAsync();
            }

            return Ok(member);
        }
        [HttpGet("sync")]
        [SwaggerOperation(Summary = "Sincroniza los Badge con las plataformas")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Sync()
        {
            var badgets = await _badGrServices.GetAllBadget();
            var mine = _context.Badges.Select(x => x.BadgeId).ToList();

            foreach (var badget in badgets.Result)
            {
                if (!mine.Any(x => x == badget.EntityId))
                {

                    _context.Badges.Add(new Badge
                    {
                        BadgeId = badget.EntityId,
                        BadgeImageUrl = badget.Image.AbsoluteUri,
                        BadgeUrl = badget.OpenBadgeId.AbsoluteUri,
                        Image = badget.Image.AbsoluteUri,
                        IssuerUrl = badget.IssuerOpenBadgeId.AbsoluteUri,
                        Name = badget.Name,
                        Description = badget.Description,
                        Created = badget.CreatedAt.Date
                    });

                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
