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
using NetBaires.Api.Handlers.Organizers;
using NetBaires.Api.Models;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizersController : ControllerBase
    {
        private readonly ILogger<SlackController> _logger;
        private readonly NetBairesContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public OrganizersController(NetBairesContext context,
            ICurrentUser currentUser,
            IMapper mapper,
            IMediator mediator,
            ILogger<SlackController> logger)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [SwaggerOperation(Summary = "Retorna todos los miembros que actualmente son organizadores de la Comunidad")]

        public async Task<IActionResult> Get() =>
            await _mediator.Send(new GetOrganizersHandler.GetOrganizers());


    }
}