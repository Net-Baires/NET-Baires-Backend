using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Handlers.Speakers;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.Speakers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SpeakersController : ControllerBase
    {
        private readonly IMediator mediator;

        public SpeakersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        [SwaggerOperation(Summary = "Retorna todos los miemebros que son/fueron speakers en la comunidad")]
        public async Task<IActionResult> Get() =>
               await mediator.Send(new GetSpeakersQuery());

    }
}