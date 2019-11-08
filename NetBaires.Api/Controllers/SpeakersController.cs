using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Api.Handlers.Badges.Models;
using NetBaires.Api.Handlers.Events;
using NetBaires.Api.Handlers.Speakers;
using NetBaires.Api.Models;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SpeakersController : ControllerBase
    {
        private readonly ILogger<SlackController> _logger;
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator mediator;

        public SpeakersController(NetBairesContext context,
            ICurrentUser currentUser,
            IMapper mapper,
            IMediator mediator,
            ILogger<SlackController> logger)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            this.mediator = mediator;
        }
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [SwaggerOperation(Summary = "Retorna todos los miemebros que son/fueron speakers en la comunidad")]
        public async Task<IActionResult> Get() =>
        await mediator.Send(new GetSpeakersHandler.GetSpeakers());

    }
}