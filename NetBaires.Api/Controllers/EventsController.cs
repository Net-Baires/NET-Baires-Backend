using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Handlers.Events;
using NetBaires.Api.Models;
using NetBaires.Api.Options;
using NetBaires.Api.Services.Meetup;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMeetupServices _meetupServices;
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IMediator _iMediator;
        private readonly AttendanceOptions _attendanceOptions;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IMeetupServices meetupServices,
            NetBairesContext context,
            ICurrentUser currentUser,
            IOptions<AttendanceOptions> attendanceOptions,
            IMediator iMediator,
            ILogger<EventsController> logger)
        {
            _meetupServices = meetupServices;
            _context = context;
            _currentUser = currentUser;
            _iMediator = iMediator;
            _attendanceOptions = attendanceOptions.Value;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna todos los eventos de la comunidad")]
        [ApiExplorerSettingsExtend("Anonymous")]
        [ProducesResponseType(typeof(Event), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get() =>
        await _iMediator.Send(new GetEventsHandler.GetEvents());

        [HttpGet("ToSync")]
        [SwaggerOperation(Summary = "Retorna todos los eventos que ya fueron sincronizados con plataformas externas, pero  no fueron procesados en nuestro sistema")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [ProducesResponseType(typeof(List<GetToAsyncResponseViewModel>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetToSync() =>
            await _iMediator.Send(new GetToSyncHandler.GetToSync());

        [HttpGet("{id}/Attendances")]
        [SwaggerOperation(Summary = "Retorna toda la información requerida por el miemebro de la comunidad para reportar su asistencia a un evento")]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        public async Task<IActionResult> GetReportAttendance(int id)
        {
            var eventToReturn = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (eventToReturn == null)
                return NotFound();

            var currentEmail = _currentUser.User.Email;
            var idUser = _currentUser.User.Id;
            var token = TokenService.Generate(_attendanceOptions.ReportAttendanceSecret, new List<Claim>
            {

                new Claim(ClaimTypes.Name, idUser.ToString()),
                new Claim(ClaimTypes.Email, currentEmail),
                new Claim("EventId", eventToReturn.Id.ToString())
            }, DateTime.Now.AddDays(5));

            return Ok(new EventAssistanceViewModel
            {
                Id = eventToReturn.Id,
                Description = eventToReturn.Description,
                Token = token,
                Title = eventToReturn.Title,
                Date = eventToReturn.Date,
                ImageUrl = eventToReturn.ImageUrl

            });
        }
        [HttpPut("Attendances/{token}")]
        [SwaggerOperation(Summary = "Valida que el token del miembro para reportar asistencia es correcto y reporta la asistencia")]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> PutReportAssistance(string token)
        {

            var response = TokenService.Validate(_attendanceOptions.ReportAttendanceSecret, token);
            if (!response.Valid)
                return BadRequest("El token indicado no es valido");

            var memberId = int.Parse(response.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value.ToString());
            var eventId = int.Parse(response.Claims.FirstOrDefault(x => x.Type == "EventId").Value.ToString());


            var eventToAdd = _context.Attendances.FirstOrDefault(x => x.EventId == eventId && x.MemberId == memberId);
            if (eventToAdd == null)
            {
                eventToAdd = new Attendance(memberId, eventId);
                await _context.Attendances.AddAsync(eventToAdd);
            }
            eventToAdd.Attend();
            await _context.SaveChangesAsync();
            return new StatusCodeResult(204);

        }
        [HttpGet("{id}/Attendances/General")]
        [SwaggerOperation(Summary = "Retorna toda la información requerida para que los miembros de la comunidad puedan reportar su asistencia en conjunto, el token de registración tiene un tiempo de 5 minutos.")]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> CheckAssistanceGeneral(int id)
        {
            var eventToReturn = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (eventToReturn == null)
                return NotFound();

            var token = TokenService.Generate(_attendanceOptions.AskAttendanceSecret, new List<Claim>
            {
                new Claim("EventId", eventToReturn.Id.ToString())
            }, DateTime.Now.AddDays(3));

            return Ok(new EventAssistanceViewModel
            {
                Id = eventToReturn.Id,
                Description = eventToReturn.Description,
                Token = token,
                Title = eventToReturn.Title,
                Date = eventToReturn.Date,
                ImageUrl = eventToReturn.ImageUrl
            });
        }
        [HttpPut("Attendances/General/{token}")]
        [SwaggerOperation(Summary = "Informa que asistió al evento mediante un token otorgado por los organizadores")]
        [AuthorizeRoles(UserRole.Member)]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        public async Task<IActionResult> PutCheckAssistanceGeneral([FromRoute]string token) =>
            await _iMediator.Send(new PutCheckAssistanceGeneralHandler.PutCheckAssistanceGeneral(token));

        [HttpGet("live")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna una lista de los eventos que se encuentra en curso.")]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public IActionResult GetLives()
        {
            // IQueryable<Event> eventToReturn = null;
            // if (_currentUser.IsLoggued)
            //     eventToReturn = _context.Events.Where(x => x.Live).AsNoTracking();
            // else
            var eventToReturn = _context.Events.Where(x => x.Live).AsNoTracking();
            if (eventToReturn != null)
                return Ok(eventToReturn);

            return NotFound();
        }

        [HttpGet("{id:int}/live")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend("Anonymous")]
        public async Task<IActionResult> GetLive([FromRoute]int id) =>
            await _iMediator.Send(new GetLiveHandler.GetLive(id));

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend("Anonymous")]
        public async Task<IActionResult> GetById([FromRoute]int id) =>
        await _iMediator.Send(new GetEventHandler.GetEvent(id));

        [HttpGet("{id:int}/attendees")]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [SwaggerOperation(Summary = "Retorna todos los miembros que se encuentran registrados a un evento particular")]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        public async Task<IActionResult> GetAttendees([FromRoute]int id)
        {
            var command = new GetAttendeesHandler.GetAttendees(id);
            return await _iMediator.Send(command);
        }

        [HttpPost("{id:int}/Members/{idMember}/attende")]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [SwaggerOperation(Summary = "Informo que un usuario se registro en un evento")]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        public async Task<IActionResult> AddAttendees([FromRoute]int id, [FromRoute]int idMember)
        {
            var command = new AddAttendeeHandler.AddAttendee(id, idMember);
            return await _iMediator.Send(command);
        }

        [HttpPut("{id:int}/Members/{idMember}/attende")]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [SwaggerOperation(Summary = "Informo que un usuario asistio a un evento. Este se registra con el estado Attended")]
        [AuthorizeRoles(new UserRole[2] { UserRole.Organizer, UserRole.Admin })]
        public async Task<IActionResult> UpdateAttendeAttended([FromRoute]int id, [FromRoute]int idMember, UpdateAttendeeHandler.UpdateAttendee attende)
        {
            attende.EventId = id;
            attende.MemberId = idMember;
            return await _iMediator.Send(attende);

        }

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettings(GroupName = "Admin")]
        public async Task<IActionResult> Put(int id, UpdateEventHandler.UpdateEvent eEvent)
        {
            eEvent.Id = id;
            return await _iMediator.Send(eEvent);
        }

        [HttpPut("sync")]
        [SwaggerOperation(Summary = "Sincroniza los Eventos con las plataformas externas y recupera su información")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Sync() =>
            await _iMediator.Send(new SyncWithExternalEventsHandler.SyncWithExternalEvents());

        [HttpPut("{id}/sync")]
        [SwaggerOperation(Summary = "Sincroniza un evento en particular con la plataforma externa")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Sync(int id) =>
            await _iMediator.Send(new SyncEventHandler.SyncEvent(id));


    }
}