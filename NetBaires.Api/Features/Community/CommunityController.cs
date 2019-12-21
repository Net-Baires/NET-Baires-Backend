﻿using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Api.Features.Badges.GetBadge;
using NetBaires.Api.Features.Slack;
using NetBaires.Data;

namespace NetBaires.Api.Features.Members
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly ILogger<SlackController> _logger;
        private readonly IMediator _mediator;

        public CommunityController(IMediator mediator,
            ILogger<SlackController> logger)
        {
            _logger = logger;
            this._mediator = mediator;
        }


        [HttpGet("summary")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> Get() =>
                     await _mediator.Send(new GetCommunitySummaryQuery());


    }
}