using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.Events.AddAttendee;
using NetBaires.Api.Features.Events.AddCurrentUserToGroupCode;
using NetBaires.Api.Features.Events.AddMaterial;
using NetBaires.Api.Features.Events.AddMemberToGroupCode;
using NetBaires.Api.Features.Events.AssignBadgeToAttendances;
using NetBaires.Api.Features.Events.CompleteEvent;
using NetBaires.Api.Features.Events.CreateGroupCode;
using NetBaires.Api.Features.Events.DeleteMemberToGroupCode;
using NetBaires.Api.Features.Events.GetAttendees;
using NetBaires.Api.Features.Events.GetDataToReportAttendanceToEvent;
using NetBaires.Api.Features.Events.GetEventLiveDetail;
using NetBaires.Api.Features.Events.GetEvents;
using NetBaires.Api.Features.Events.GetInfoToCheckAttendanceGeneral;
using NetBaires.Api.Features.Events.GetLinkEventLive;
using NetBaires.Api.Features.Events.GetSpeakersInEvent;
using NetBaires.Api.Features.Events.PutCheckAttendanceGeneralByCode;
using NetBaires.Api.Features.Events.PutCheckAttendanceGeneralByToken;
using NetBaires.Api.Features.Events.PutReportAttendance;
using NetBaires.Api.Features.Events.SyncEvent;
using NetBaires.Api.Features.Events.SyncWithExternalEvents;
using NetBaires.Api.Features.Events.UpdateAttendee;
using NetBaires.Api.Features.Events.UpdateEvent;
using NetBaires.Api.Features.Materials.RemoveMaterial;
using NetBaires.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.Materials
{
    [Authorize]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMediator _iMediator;

        public MaterialsController(IMediator iMediator)
        {
            _iMediator = iMediator;
        }


        [HttpPost("Events/{eventId}/Materials")]
        [SwaggerOperation(Summary = "Agrega material al evento")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> PutCheckAttendanceGeneral([FromRoute]int eventId, [FromBody] AddMaterialCommand command)
        {
            command.EventId = eventId;
            return await _iMediator.Send(command);
        }

        [HttpDelete("Events/{eventId}/Materials/{MaterialId}")]
        [SwaggerOperation(Summary = "Agrega material al evento")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> PutCheckAttendanceGeneral(int eventId, int materialId)
        {
            return await _iMediator.Send(new RemoveMaterialCommand(eventId,materialId));
        }
    }
}