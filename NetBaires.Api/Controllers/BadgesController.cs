using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Handlers.Badges;
using NetBaires.Api.Handlers.Badges.Models;
using NetBaires.Api.Services.BadGr;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Controllers
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            return buffer;
        }
    }
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BadgesController : ControllerBase
    {
        private readonly NetBairesContext _context;
        private readonly IMediator _mediator;
        private readonly IBadGrServices _badGrServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<BadgesController> _logger;

        public BadgesController(IHttpClientFactory httpClientFactory,
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
        [SwaggerOperation(Summary = "Retorna todos los badges disponibles de NET-Baires")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public async Task<IActionResult> GetAsync() =>
        await _mediator.Send(new GetBadgesHandler.GetBages());

        [HttpGet("{badgeId}")]
        [SwaggerOperation(Summary = "Retorna todos los badges disponibles de NET-Baires")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public async Task<IActionResult> Get(int badgeId)
        {
            var command = new GetBadeHandler.GetBadge(badgeId);
            return await _mediator.Send(command);
        }
        [HttpGet("{badgeId}/image")]
        [SwaggerOperation(Summary = "Retorna la imagen del Badge")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public async Task<IActionResult> GetImage(int badgeId)
        {
            var response = await _mediator.Send(new GetImageHandler.GetIamge(badgeId));
            return File(response.ToByteArray(), "image/png");
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
            //var response = await _badGrServices.CreateAssertion(badge.BadgeId, member.Email);

            _context.BadgeMembers.Add(new BadgeMember
            {
                BadgeId = badge.Id,
                MemberId = member.Id,
                BadgeUrl = ""
            });

            return Ok(member);
        }
        [HttpDelete("{badgeId}/Member/{memberId}")]
        [SwaggerOperation(Summary = "Eliminar un Badge de un Miembro")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> RemoveMembersInBadge([FromRoute]int badgeId, [FromRoute]int memberId)
        {
            var badge = await _context.BadgeMembers.FirstOrDefaultAsync(x => x.BadgeId == badgeId && x.MemberId == memberId);
            if (badge == null)
                return BadRequest("El miembro que esta intentando asignar ya tiene ese Badge");

            _context.Remove(badge);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [ProducesResponseType(typeof(NewBadgeHandler.NewBadgeResponse), 200)]
        public async Task<IActionResult> Post([FromForm]NewBadgeHandler.NewBadge badge)
        {
            return await _mediator.Send(badge);
        }

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [ProducesResponseType(typeof(UpdateBadgeHandler.UpdateBadgeResponse), 200)]
        public async Task<IActionResult> Put([FromRoute]int id, [FromForm]UpdateBadgeHandler.UpdateBadge badge)
        {
            badge.Id = id;

            return await _mediator.Send(badge);
        }
        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteBadge([FromRoute]int id) =>
            await _mediator.Send(new DeleteBadgeHandler.DeleteBadge(id));
    }
}
