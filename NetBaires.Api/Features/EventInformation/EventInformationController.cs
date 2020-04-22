using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.EventInformation.AddEventInformation;
using NetBaires.Api.Features.EventInformation.GetEventInformation;
using NetBaires.Api.Features.EventInformation.RemovEventInformation;
using NetBaires.Api.Features.EventInformation.UpdateEventInformation;
using NetBaires.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.EventInformation
{
    [Authorize]
    [ApiController]
    public class EventInformationController : ControllerBase
    {
        private readonly IMediator _iMediator;

        public EventInformationController(IMediator iMediator)
        {
            _iMediator = iMediator;
        }

        [HttpGet("Events/{eventId}/information")]
        [SwaggerOperation(Summary = "Retorna toda la información adicional de un evento")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer, UserRole.Member)]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        public async Task<IActionResult> GetEventInformation([FromRoute] int eventId) =>
            await _iMediator.Send(new GetEventInformationQuery(eventId));


        [HttpPost("Events/{eventId}/Information")]
        [SwaggerOperation(Summary = "Agrega Información adicional a un evento")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> AddEventInformation([FromRoute]int eventId, [FromBody] AddEventInformationCommand command)
        {
            command.EventId = eventId;
            return await _iMediator.Send(command);
        }

        [HttpDelete("Events/{eventId}/Information/{InformationId}")]
        [SwaggerOperation(Summary = "Agrega material al evento")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> RemoveEventInformation(int eventId, int informationId)
        {
            return await _iMediator.Send(new RemoveEventInformationCommand(eventId, informationId));
        }

        [HttpPut("Events/{eventId}/Information/{InformationId}")]
        [SwaggerOperation(Summary = "Actualiza la información de un evento")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> PutCheckAttendanceGeneral(int eventId, int eventInformationId,[FromBody]UpdateEventInformationCommand command)
        {
            command.Id = eventInformationId;
            return await _iMediator.Send(new UpdateEventInformationCommand());
        }
    }
}