using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.Events.AssignBadgeToAttendances;
using NetBaires.Api.Features.Events.CompleteEvent;
using NetBaires.Api.Features.Events.GetAttendees;
using NetBaires.Api.Features.Events.GetDataToReportAttendanceToEvent;
using NetBaires.Api.Features.Events.GetEventLiveDetail;
using NetBaires.Api.Features.Events.GetEvents;
using NetBaires.Api.Features.Events.GetInfoToCheckAttendanceGeneral;
using NetBaires.Api.Features.Events.PutCheckAttendanceByCode;
using NetBaires.Api.Features.Events.PutReportAttendance;
using NetBaires.Api.Features.Events.SyncEvent;
using NetBaires.Api.Features.Events.SyncWithExternalEvents;
using NetBaires.Api.Features.Events.UpdateAttendee;
using NetBaires.Api.Features.Events.UpdateEvent;
using NetBaires.Api.Features.GroupsCodes.AddMemberToGroupCode;
using NetBaires.Api.Features.GroupsCodes.DeleteGroupCode;
using NetBaires.Api.Features.GroupsCodes.GetGroupCode;
using NetBaires.Api.Features.GroupsCodes.UpdateGroupCode;
using NetBaires.Api.Handlers.Events;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.GroupsCodes
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GroupCodesController : ControllerBase
    {
        private readonly IMediator _iMediator;

        public GroupCodesController(IMediator iMediator)
        {
            _iMediator = iMediator;
        }
        [HttpPut("{groupCodeId:int}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> UpdateGroupCode([FromRoute]int groupCodeId, UpdateGroupCodeCommand command)
        {
            command.GroupCodeId = groupCodeId;
            return await _iMediator.Send(command);
        }

        [HttpGet("{groupCodeId}")]
        [SwaggerOperation(Summary = "Retorna el detalle de un Group Code")]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        public async Task<IActionResult> GetGroupCode([FromRoute]GetGroupCodeQuery query) =>
            await _iMediator.Send(query);

        [HttpDelete("{groupCodeId}")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> DeleteGroupCode([FromRoute]DeleteGroupCodeCommand query) =>
            await _iMediator.Send(query);

        [HttpPost("{groupCodeId}/badges/{badgeId}")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        public async Task<IActionResult> AssignBadgeToAttendancesInGroupCode([FromRoute]AssignBadgeToAttendancesInGroupCodeCommand command) =>
            await _iMediator.Send(command);

        [HttpPost("{groupCodeId}/raffle")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        public async Task<IActionResult> MakeRaffle([FromBody]MakeRaffleCommand command) =>
             await _iMediator.Send(command);



    }
}
