using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly IBadGrServices _badGrServices;
        private readonly ILogger<BadgesController> _logger;

        public BadgesController(IHttpClientFactory httpClientFactory,
            NetBairesContext context,
            IBadGrServices badGrServices,
            ILogger<BadgesController> logger)
        {
            _context = context;
            _badGrServices = badGrServices;
            _logger = logger;
            httpClientFactory.CreateClient();
        }

        [HttpGet("{email}")]
        [SwaggerOperation(Summary = "Retorna todos los badges recibidos por el usuario")]
        [AllowAnonymous]
        public IActionResult Get([FromRoute] string email)
        {
            var badges = _context.BadgeMembers.Where(x => x.User.Email.ToUpper() == email.ToUpper())
                .Select(x =>
                    new BadgeViewModel(x.Badge, x.BadgeUrl))
                .AsNoTracking();
            return Ok(badges);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retorna todos los badges disponibles de NET-Baires")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var badges = _context.Badges.AsNoTracking();

            return Ok(badges);
        }

        [HttpGet("{badgeId}/Members")]
        [SwaggerOperation(Summary = "Retorna de la lista de usuario que recibieron el Badge")]
        public async Task<IActionResult> GetMembersInBadge(int badgeId)
        {
            var users = _context.Members.Where(x => x.Badges.Any(s => s.BadgeId == badgeId)).AsNoTracking();
            return Ok(users);
        }

        [HttpPost("{badgeId}/Member/{memberId}")]
        [SwaggerOperation(Summary = "Premia a un miembro con un Badge")]
        [AuthorizeRoles(UserRole.Admin)]
        public async Task<IActionResult> AssignMembersInBadge([FromRoute]int badgeId, [FromRoute]int memberId)
        {
            var membersAlreadyHasTheBadge =
                 _context.BadgeMembers.Any(x => x.BadgeId == badgeId && x.UserId == memberId);
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
                    UserId = member.Id,
                    BadgeUrl = response.Result.First().OpenBadgeId.AbsoluteUri
                });
                await _context.SaveChangesAsync();
            }

            return Ok(member);
        }

        [HttpGet("sync")]
        [SwaggerOperation(Summary = "Sincroniza los Badge con las plataformas")]
        [AuthorizeRoles(UserRole.Admin)]

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
                        Description = badget.Description
                    });

                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    
    }
}
