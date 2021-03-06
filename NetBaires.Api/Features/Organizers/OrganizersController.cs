﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.Organizers.GetOrganizers;
using NetBaires.Data;
using NetBaires.Data.Entities;
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