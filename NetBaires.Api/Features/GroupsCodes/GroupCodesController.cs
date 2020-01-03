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



        [HttpPost("{groupCodeId:int}/{code}")]
        [SwaggerOperation(Summary = "Agrega al usuario logueado al group de codigo")]
        [AuthorizeRoles(UserRole.Member)]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        public async Task<IActionResult>
            AddMemberToGroupCode([FromRoute]int groupCodeId, [FromRoute] string code) =>
            await _iMediator.Send(new AddMemberToGroupCodeCommand { Code = code, GroupCodeId = groupCodeId });

    }
}
