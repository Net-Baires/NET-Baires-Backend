using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Api.Handlers.Badges.Models;
using NetBaires.Api.Models;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly ILogger<SlackController> _logger;
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;

        public MembersController(NetBairesContext context,
            ICurrentUser currentUser,
            IMapper mapper,
            ILogger<SlackController> logger)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [SwaggerOperation(Summary = "Retorna todos los miembros activos de la Comunidad")]

        public async Task<IActionResult> Get()
        {
            var member = _context.Members.AsNoTracking();

            if (member != null)
                return Ok(member);

            return NotFound();
        }

        [HttpGet("{idMember:int}")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> GetById([FromRoute]int idMember)
        {
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Id == idMember);

            if (member != null)
                return Ok(member);

            return NotFound();
        }

        [HttpGet("{email}")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> Get([FromRoute]string email)
        {
            var member = await _context.Members.FirstOrDefaultAsync(x => x.Email.ToUpper() == email.ToUpper());

            if (member != null)
                return Ok(member);

            return NotFound();
        }


        [HttpGet("{email}/badges")]
        [SwaggerOperation(Summary = "Retorna todos los badges recibidos por el miembro")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<BadgeViewModel>), 200)]
        public IActionResult GetBadgesFromEmail([FromRoute] string email)
        {
            var badges = _context.BadgeMembers.Where(x => x.Member.Email.ToUpper() == email.ToUpper())
                .Select((x) => _mapper.Map(x.Badge, new BadgeDetailViewModel()));
            return Ok(badges);
        }
        [HttpPost]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Post(Member member)
        {
            if (_context.Members.Any(x => x.Email.ToUpper() == member.Email.ToUpper()))
                return BadRequest("Ya se encuentra un usuario registrado con ese email");

            member.Role = UserRole.Member;
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
            return Ok(member);
        }
        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Put(int id, Member member)
        {
            member.Id = id;

            _context.Entry(member).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(member);
        }
        [HttpPut("{id}/Events/{eventId}/Assistance/")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Put(int id, int eventId, bool assistance)
        {
            var eventMember = await _context.EventMembers.FirstOrDefaultAsync(x => x.MemberId == id
                                                                            &&
                                                                            x.EventId == eventId);
            if (eventMember == null)
            {
                eventMember = new EventMember(id, eventId);
                await _context.EventMembers.AddAsync(eventMember);
            }
            if (assistance)
                eventMember.Attend();
            else
                eventMember.NoAttend();

            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var sponsorToDelete = await _context.Members.FirstOrDefaultAsync(x => x.Id == id);
            if (sponsorToDelete == null)
                return NotFound();
            _context.Members.Remove(sponsorToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}