﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Handlers.Me;
using NetBaires.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.Me
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
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
         await _iMediator.Send(new GetMeQuery());

        [HttpPut]
        [SwaggerOperation(Summary = "Actualiza la información del usuario logueado")]
        [ProducesResponseType(typeof(Event), 200)]
        public async Task<IActionResult> Update([FromForm]UpdateMeCommand me) =>
           await _iMediator.Send(me);

    }
}