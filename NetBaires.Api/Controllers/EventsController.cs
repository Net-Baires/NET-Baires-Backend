using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Models;
using NetBaires.Api.Options;
using NetBaires.Api.Services.Meetup;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMeetupServices _meetupServices;
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly AssistanceOptions _assistanceOptions;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IMeetupServices meetupServices,
            NetBairesContext context,
            ICurrentUser currentUser,
            IOptions<AssistanceOptions> assistanceOptions,
            ILogger<EventsController> logger)
        {
            _meetupServices = meetupServices;
            _context = context;
            _currentUser = currentUser;
            _assistanceOptions = assistanceOptions.Value;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna todos los eventos de la comunidad")]

        public async Task<IActionResult> Get()
        {
            var eventToReturn = _context.Events.OrderByDescending(x => x.Id).AsNoTracking();

            if (eventToReturn != null)
                return Ok(eventToReturn);

            return NotFound();
        }
        [HttpGet("ToSync")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna todos los eventos que ya fueron sincronizados con plataformas externas, pero  no fueron procesados en nuestro sistema")]
        public IActionResult GetToSync()
        {
            var eventToReturn = _context.Events.OrderByDescending(x => x.Id).Where(x => !x.Done).AsNoTracking();

            if (eventToReturn != null)
                return Ok(eventToReturn);

            return NotFound();
        }
        [HttpGet("{id}/Assistance")]
        [SwaggerOperation(Summary = "Retorna toda la información requerida por el miemebro de la comunidad para reportar su asistencia a un evento")]
        public async Task<IActionResult> GetReportAssistance(int id)
        {
            var eventToReturn = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (eventToReturn == null)
                return NotFound();

            var currentEmail = _currentUser.User.Email;
            var idUser = _currentUser.User.Id;
            var token = TokenService.Generate(_assistanceOptions.ReportAssistanceSecret, new List<Claim>
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
        [HttpPut("/Assistance/{token}")]
        [SwaggerOperation(Summary = "Valida que el token del miembro para reportar asistencia es correcto y reporta la asistencia")]
        [AuthorizeRoles(UserRole.Admin)]
        public async Task<IActionResult> PutReportAssistance(string token)
        {

            var response = TokenService.Validate(_assistanceOptions.ReportAssistanceSecret, token);
            if (!response.Valid)
                return BadRequest("El token indicado no es valido");

            var memberId = int.Parse(response.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value.ToString());
            var eventId = int.Parse(response.Claims.FirstOrDefault(x => x.Type == "EventId").Value.ToString());

            if (_context.EventMembers.Any(x => x.EventId == eventId && x.MemberId == memberId))
                return Conflict("El usuario que intenta registrar ya se encuentra registrado en el evento");

            await _context.EventMembers.AddAsync(EventMember.Attend(memberId, eventId));
            await _context.SaveChangesAsync();
            return Ok();

        }
        [HttpGet("{id}/Assistance/General")]
        [SwaggerOperation(Summary = "Retorna toda la información requerida para que los miembros de la comunidad puedan reportar su asistencia en conjunto, el token de registración tiene un tiempo de 5 minutos.")]
        public async Task<IActionResult> CheckAssistanceGeneral(int id)
        {
            var eventToReturn = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (eventToReturn == null)
                return NotFound();

            var token = TokenService.Generate(_assistanceOptions.AskAssistanceSecret, new List<Claim>
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
        [HttpPut("Assistance/General")]
        [SwaggerOperation(Summary = "Informa que asistió al evento mediante un token otorgado por los organizadores")]
        public async Task<IActionResult> PutCheckAssistanceGeneral(string token)
        {
            var response = TokenService.Validate(_assistanceOptions.AskAssistanceSecret, token);
            if (!response.Valid)
                return BadRequest("El token indicado no es valido");


            var eventId = int.Parse(response.Claims.FirstOrDefault(x => x.Type == "EventId").Value.ToString());
            var memberId = _currentUser.User.Id;
            if (_context.EventMembers.Any(x => x.EventId == eventId && x.MemberId == memberId))
                return Conflict("Ya se encuentra registrado al evento.");

            await _context.EventMembers.AddAsync(EventMember.Attend(memberId, eventId));
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("live")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retorna una lista de los eventos que se encuentra en curso.")]

        public IActionResult GetLives()
        {
            IQueryable<Event> eventToReturn = null;
            if (_currentUser.IsLoggued)
                eventToReturn = _context.Events.Where(x => x.Live && !x.Attendees.Any(s => s.MemberId == _currentUser.User.Id)).AsNoTracking();
            else
                eventToReturn = _context.Events.Where(x => x.Live).AsNoTracking();
            if (eventToReturn != null)
                return Ok(eventToReturn);

            return NotFound();
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var eventToReturn = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);

            if (eventToReturn != null)
                return Ok(eventToReturn);

            return NotFound();
        }
        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        public async Task<IActionResult> Put(int id, Event eEvent)
        {
            eEvent.Id = id;

            _context.Entry(eEvent).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(eEvent);
        }

        [HttpGet("sync")]
        [SwaggerOperation(Summary = "Sincroniza los Eventos con las plataformas")]
        [AuthorizeRoles(UserRole.Admin)]
        public async Task<IActionResult> Sync()
        {
            var eventsToAdd = await _meetupServices.GetAllEvents();
            var mines = _context.Events.Select(x => x.EventId).ToList();

            foreach (var eventToAdd in eventsToAdd)
            {
                if (!mines.Any(x => x == eventToAdd.Id.ToString()))
                {
                    _context.Events.Add(new Event()
                    {
                        Description = eventToAdd.Description,
                        Title = eventToAdd.Name,
                        Url = eventToAdd.Link.AbsoluteUri,
                        EventId = eventToAdd.Id.ToString(),
                        Date = eventToAdd.LocalDate.Date,
                        ImageUrl = eventToAdd?.FeaturedPhoto?.HighresLink?.AbsoluteUri
                    });

                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}