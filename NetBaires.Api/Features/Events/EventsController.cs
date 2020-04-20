using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.Events.AddAttendee;
using NetBaires.Api.Features.Events.AddMemberToGroupCode;
using NetBaires.Api.Features.Events.AssignBadgeToAttendances;
using NetBaires.Api.Features.Events.CompleteEvent;
using NetBaires.Api.Features.Events.DeleteMemberToGroupCode;
using NetBaires.Api.Features.Events.GetAttendees;
using NetBaires.Api.Features.Events.GetDataToReportAttendanceToEvent;
using NetBaires.Api.Features.Events.GetEventLiveDetail;
using NetBaires.Api.Features.Events.GetEvents;
using NetBaires.Api.Features.Events.GetInfoToCheckAttendanceGeneral;
using NetBaires.Api.Features.Events.GetLinkEventLive;
using NetBaires.Api.Features.Events.GetSpeakersInEvent;
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

namespace NetBaires.Api.Features.Events
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _iMediator;

        public EventsController(IMediator iMediator)
        {
            _iMediator = iMediator;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna un evento de la comunidad")]
        [ApiExplorerSettingsExtend("Anonymous")]
        [ProducesResponseType(typeof(Event), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById([FromRoute]int id, [FromQuery]bool? done, [FromQuery]bool? live) =>
             await _iMediator.Send(new GetEventsQuery(done, live, id));

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna todos los eventos de la comunidad")]
        [ApiExplorerSettingsExtend("Anonymous")]
        [ProducesResponseType(typeof(Event), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAll([FromQuery]bool? done, [FromQuery]bool? live, [FromQuery]bool? upcoming) =>
                await _iMediator.Send(new GetEventsQuery(done, live, upcoming));


        [HttpGet("{id}/Live/Detail")]
        [SwaggerOperation(Summary = "Retorna el detalle de un evento en vivo")]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        [AuthorizeRoles(UserRole.Organizer, UserRole.Admin, UserRole.Member)]
        public async Task<IActionResult> GetEventLiveDetail([FromRoute]int id) =>
            await _iMediator.Send(new GetEventLiveDetailQuery(id));

        [HttpGet("{id}/Speakers")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna todos los Speakers de un evento")]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        [ProducesResponseType(typeof(GetSpeakersInEventInEventResponse), 200)]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetSpeakersInEvent([FromRoute]int id) =>
            await _iMediator.Send(new GetSpeakersInEventQuery(id));


        [HttpGet("Live/Link")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna el link de la transmisión en vivo")]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [ProducesResponseType(typeof(GetLinkEventLiveQuery.Response), 200)]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetEventLiveLink() =>
            await _iMediator.Send(new GetLinkEventLiveQuery());

        [HttpGet("{id}/Attendance")]
        [SwaggerOperation(Summary = "Retorna toda la información requerida por el miembro de la comunidad para reportar su asistencia a un evento particular")]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        //[AuthorizeRoles(new UserRole[1] { UserRole.Admin})]
        [Authorize]
        public async Task<IActionResult> GetDataToReportAttendanceToEvent([FromRoute]GetDataToReportAttendanceToEventCommand command) =>
            await _iMediator.Send(command);

        [HttpPut("Attendances/{token}")]
        [SwaggerOperation(Summary = "Valida que el token del miembro para reportar asistencia es correcto y reporta la asistencia")]
        [AuthorizeRoles(UserRole.Organizer, UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> PutReportAttendance([FromRoute]string token) =>
            await _iMediator.Send(new PutReportAttendanceCommand { Token = token });

        [HttpGet("{id}/Attendances/General")]
        [SwaggerOperation(Summary = "Retorna toda la información requerida para que los miembros de la comunidad puedan reportar su asistencia en conjunto, el token de registración tiene un tiempo de 5 minutos.")]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> InfoToCheckAttendanceGeneral([FromRoute]GetInfoToCheckAttendanceGeneralCommand command) =>
            await _iMediator.Send(command);

        [HttpPut("Attendances/General/{token}")]
        [SwaggerOperation(Summary = "Informa que asistió al evento mediante un token otorgado por los organizadores")]
        [AuthorizeRoles(UserRole.Member)]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        public async Task<IActionResult> PutCheckAttendanceGeneral([FromRoute]PutCheckAttendanceGeneralCommand command) =>
            await _iMediator.Send(command);

        [HttpPut("{eventId:int}/Attendances/General/{code}")]
        [SwaggerOperation(Summary = "Informa que asistió al evento mediante un código entregado por los organizadores")]
        [AuthorizeRoles(UserRole.Member)]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        public async Task<IActionResult> PutCheckAttendanceByCode([FromRoute] int eventId, [FromRoute]string code) =>
            await _iMediator.Send(new PutCheckAttendanceByCodeCommand(eventId, code));

        [HttpGet("{eventId:int}/attendees")]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [SwaggerOperation(Summary = "Retorna todos los miembros que se encuentran registrados a un evento particular")]
        [AuthorizeRoles(UserRole.Admin)]
        public async Task<IActionResult> GetAttendees([FromRoute]int eventId, [FromQuery]int? memberId, [FromQuery]string query)
        {
            return await _iMediator.Send(new GetAttendeesQuery(eventId, memberId, query));
        }

        [HttpPost("{idEvent:int}/Members/{idMember}/attende")]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [SwaggerOperation(Summary = "Informo que un usuario se registro en un evento")]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        public async Task<IActionResult> AddAttendees([FromRoute]AddAttendeeCommand command) =>
                await _iMediator.Send(command);

        [HttpPut("{id:int}/Members/{idMember}/attende")]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [SwaggerOperation(Summary = "Informo que un usuario asistio a un evento. Este se registra con el estado Attended")]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        public async Task<IActionResult> UpdateAttendeAttended([FromRoute]int id, [FromRoute]int idMember, UpdateAttendeeCommand command)
        {
            command.EventId = id;
            command.MemberId = idMember;
            return await _iMediator.Send(command);

        }

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> UpdateEvent(int id, UpdateEventCommand command)
        {
            command.Id = id;
            return await _iMediator.Send(command);
        }

        [HttpPut("sync")]
        [SwaggerOperation(Summary = "Sincroniza los Eventos con las plataformas externas y recupera su información")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> SyncWithExternalEvents() =>
            await _iMediator.Send(new SyncWithExternalEventsCommand());

        [HttpPut("{id}/sync")]
        [SwaggerOperation(Summary = "Sincroniza un evento en particular con la plataforma externa")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Sync(int id) =>
            await _iMediator.Send(new SyncEventHandler.SyncEvent(id));

        [HttpPut("{id}/done")]
        [SwaggerOperation(Summary = "Cambia el estado de un evento a Completado")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> CompleteEvent([FromRoute] CompleteEventCommand command) =>
                     await _iMediator.Send(command);

        [HttpPost("{eventId:int}/badges/{badgeId:int}/Attendances")]
        [SwaggerOperation(Summary = "Sincroniza un evento en particular con la plataforma externa")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> AssignBadgeToAttendances([FromRoute]AssignBadgeToAttendancesCommand command) =>
            await _iMediator.Send(command);

        [HttpPost("{eventId:int}/groupcodes")]
        [SwaggerOperation(Summary = "Crea un GroupCode para un evento especifico")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> CreateGroupCode([FromRoute] int eventId, [FromBody]CreateGroupCodeCommand command) =>
            await _iMediator.Send(new CreateGroupCodeCommand { EventId = eventId, Detail = command.Detail });

        [HttpPost("{eventId:int}/groupcodes/{code}")]
        [SwaggerOperation(Summary = "Agrega al usuario logueado al group de codigo")]
        [AuthorizeRoles(UserRole.Member)]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        public async Task<IActionResult>
            AddCurrentUserToGroupCode([FromRoute]int eventId, [FromRoute] string code) =>
            await _iMediator.Send(new AddCurrentUserToGroupCodeCommand { Code = code, EventId = eventId });

        [HttpPost("{eventId:int}/groupcodes/{groupCodeId}/members/{memberId}")]
        [SwaggerOperation(Summary = "Agrega un miembro al grupo de código.")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult>
            AddMemberToGroupCode([FromRoute]AddMemberToGroupCodeCommand command) =>
            await _iMediator.Send(command);

        [HttpDelete("{eventId:int}/groupcodes/{groupCodeId}/members/{memberId}")]
        [SwaggerOperation(Summary = "Remueve un miembro de un groupo de código")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult>
            DeleteMemberToGroupCode([FromRoute]DeleteMemberToGroupCodeCommand command) =>
            await _iMediator.Send(command);

    }
}