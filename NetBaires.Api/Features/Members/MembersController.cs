using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Api.Features.Members.AddMember;
using NetBaires.Api.Features.Members.FollowMember;
using NetBaires.Api.Features.Members.GetBadgeFromMember;
using NetBaires.Api.Features.Members.GetBadgesFromMember;
using NetBaires.Api.Features.Members.GetFollowingsFromMember;
using NetBaires.Api.Features.Members.GetMemberDetail;
using NetBaires.Api.Features.Members.InformAttendances;
using NetBaires.Api.Features.Members.SearchMember;
using NetBaires.Api.Features.Members.UnFollowMember;
using NetBaires.Api.Features.Members.UpdateInformation;
using NetBaires.Api.Features.Slack;
using NetBaires.Api.ViewModels;
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

        [HttpGet("{id:int}/followings")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> GetById([FromRoute]GetFollowingsFromMemberQuery query) =>
            await _mediator.Send(query);

        [HttpGet("{query}")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> Get([FromRoute]string query) =>
                     await _mediator.Send(new SearchMemberQuery(query));


        [HttpGet("badges")]
        [SwaggerOperation(Summary = "Retorna todos los badges recibidos por el miembro")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<BadgeDetailViewModel>), 200)]
        public async Task<IActionResult> GetBadgesFromEmailAsync([FromQuery] string email)
            => await _mediator.Send(new GetBadgesFromMemberQuery(email));

        [HttpGet("{id:int}/badges/{badgeId:int}")]
        [SwaggerOperation(Summary = "Retorna todos los badges recibidos por el miembro")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<BadgeDetailViewModel>), 200)]
        public async Task<IActionResult> GetBadgeFromMember([FromRoute]GetBadgeFromMemberQuery query)
            => await _mediator.Send(query);


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
        public async Task<IActionResult> AddMember(AddMemberCommand command)
            => await _mediator.Send(command);

        [HttpPost("{id}/Follow")]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        public async Task<IActionResult> FollowMember([FromRoute]int id)
            => await _mediator.Send(new FollowMemberCommand(id));

        [HttpDelete("{id}/UnFollow")]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        public async Task<IActionResult> UnFollowMember([FromRoute]int id)
            => await _mediator.Send(new UnFollowMemberCommand(id));

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> UpdateMember(int id, Member member)
        {
            member.Id = id;
            if (member.Organized)
                member.Role = UserRole.Organizer;
            else member.Role = UserRole.Member;
            _context.Entry(member).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(member);
        }

        [HttpPut("information")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer, UserRole.Member)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> AddPushNotificationId(UpdateInformationCommand command) =>
            await _mediator.Send(command);

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