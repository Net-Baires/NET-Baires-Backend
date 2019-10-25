using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Handlers.Events;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MeController : ControllerBase
    {
        private readonly IMediator _iMediator;

        public MeController(IMediator iMediator)
        {
            _iMediator = iMediator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retorna toda la información de perfil del usuario autenticado")]
        [ProducesResponseType(typeof(Event), 200)]
        public async Task<IActionResult> Get() =>
         await _iMediator.Send(new GetMeHandler.GetMe());

        [HttpPut]
        [SwaggerOperation(Summary = "Actualiza la información del usuario logueado")]
        [ProducesResponseType(typeof(Event), 200)]
        public async Task<IActionResult> Update([FromForm]UpdateMeHandler.UpdateMe me) =>
           await _iMediator.Send(me);

    }
}