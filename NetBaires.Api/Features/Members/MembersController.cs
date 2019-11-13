using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Api.Features.Badges.AssignMembersToBadge;
using NetBaires.Api.Features.Badges.GetBadge;
using NetBaires.Api.Features.Badges.Models;
using NetBaires.Api.Features.Slack;
using NetBaires.Api.Models;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.Members
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly ILogger<SlackController> _logger;
        private readonly NetBairesContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MembersController(NetBairesContext context,
            ICurrentUser currentUser,
            IMediator mediator,
            IMapper mapper,
            ILogger<SlackController> logger)
        {
            _logger = logger;
            _context = context;
            this._mediator = mediator;
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

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> GetById([FromRoute]GetMemberDetailQuery query) =>
                     await _mediator.Send(query);

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


        [HttpGet("badges")]
        [SwaggerOperation(Summary = "Retorna todos los badges recibidos por el miembro")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<BadgeDetailViewModel>), 200)]
        public async Task<IActionResult> GetBadgesFromEmailAsync([FromQuery] string email)
            => await _mediator.Send(new GetBadgesFromMemberQuery(email));

        [HttpGet("{id:int}/badges")]
        [SwaggerOperation(Summary = "Retorna todos los badges recibidos por el miembro")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<BadgeDetailViewModel>), 200)]
        public async Task<IActionResult> GetBadgesFromEmailAsync([FromRoute] int id)
                      => await _mediator.Send(new GetBadgesFromMemberQuery(id));

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
            if (member.Organized)
                member.Role = UserRole.Organizer;
            else member.Role = UserRole.Member;
            _context.Entry(member).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(member);
        }
        [HttpPut("{id}/Events/{eventId}/Attendances/{attended}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Put([FromRoute]InformAttendancesCommand command) =>
            await _mediator.Send(command);

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