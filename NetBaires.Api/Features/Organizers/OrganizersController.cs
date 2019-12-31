using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Auth;
using NetBaires.Api.Features.Organizers.GetOrganizers;
using NetBaires.Api.Features.Slack;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.Organizers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrganizersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrganizersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [SwaggerOperation(Summary = "Retorna todos los miembros que actualmente son organizadores de la Comunidad")]

        public async Task<IActionResult> Get() =>
            await _mediator.Send(new GetOrganizersQuery());


    }
}