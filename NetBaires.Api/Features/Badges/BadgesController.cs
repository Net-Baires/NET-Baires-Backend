using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Badges.AssignMembersToBadge;
using NetBaires.Api.Features.Badges.DeleteBadge;
using NetBaires.Api.Features.Badges.GetBadges;
using NetBaires.Api.Features.Badges.GetMembersInBadge;
using NetBaires.Api.Features.Badges.NewBadge;
using NetBaires.Api.Features.Badges.UpdateBadge;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using NetBaires.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.Badges
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
        private readonly ILogger<BadgesController> _logger;

        public BadgesController(IHttpClientFactory httpClientFactory,
            NetBairesContext context,
            IMediator mediator,
            ILogger<BadgesController> logger)
        {
            _context = context;
            _mediator = mediator;

            _logger = logger;
            httpClientFactory.CreateClient();
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Retorna todos los badges disponibles de NET-Baires")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public async Task<IActionResult> GetAsync() =>
              await _mediator.Send(new GetBadgeQuery());

        [HttpGet("{badgeId}")]
        [SwaggerOperation(Summary = "Retorna el detalle de un badge de NET-Baires")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(List<Badge>), 200)]
        public async Task<IActionResult> Get(int badgeId)=>
            await _mediator.Send(new GetBadgeQuery(badgeId));

        [HttpGet("{badgeId}/Members")]
        [SwaggerOperation(Summary = "Retorna de la lista de usuario que recibieron el Badge")]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        [ProducesResponseType(typeof(List<MemberDetailViewModel>), 200)]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> GetMembersInBadge([FromRoute]GetMembersInBadgeQuery query) =>
            await _mediator.Send(query);

        [HttpPost("{badgeId}/Members/{memberId}")]
        [SwaggerOperation(Summary = "Premia a un miembro con un Badge")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> AssignMemberToBadge([FromRoute]AssignMemberToBadgeCommand query) =>
            await _mediator.Send(query);
        
        
        [HttpDelete("{badgeId}/Members/{memberId}")]
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
        public async Task<IActionResult> NewBadge([FromForm]NewBadgeCommand badge)=>
            await _mediator.Send(badge);

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> UpdateBadge([FromRoute]int id, [FromForm]UpdateBadgeCommand badge)
        {
            badge.Id = id;
            return await _mediator.Send(badge);
        }
        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteBadge([FromRoute]int id) =>
            await _mediator.Send(new DeleteBadgeCommand(id));
    }
}
