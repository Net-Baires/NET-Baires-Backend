using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Models;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlackController : ControllerBase
    {

        private readonly ILogger<SlackController> _logger;
        private readonly HttpClient _client;
        private readonly IOptions<SlackEndPointOptions> _slackEndPoint;

        public SlackController(IHttpClientFactory httpClientFactory,
            IOptions<SlackEndPointOptions> slackEndPoint,
            ILogger<SlackController> logger)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://slack.com/api/");
            _slackEndPoint = slackEndPoint;
        }

        [HttpPost]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> Join([FromBody] JoinSlackModel model)
        {
            if (ModelState.IsValid)
            {

                var dict = new Dictionary<string, string>
                {
                    {"token", _slackEndPoint.Value.Token},
                    {"email", model.Email}
                };
                var req = new HttpRequestMessage(HttpMethod.Post, _slackEndPoint.Value.Url)
                {
                    Content = new FormUrlEncodedContent(dict)
                };
                req.Headers.Add("Accept", "application/json");
                var result = await _client.SendAsync(req);
                var slackResponse = await result.Content.ReadAsAsync<SlackInviteResponseViewModel>();
                string error;
                if (!slackResponse.Ok)
                {
                    switch (slackResponse.Error)
                    {
                        case "already_invited":
                            error = "Este email ya se encuentra invitado a nuestro Slack";
                            break;
                        case "already_in_team_invited_user":
                            error = "Este email ya se encuentra invitado a nuestro Slack";
                            break;
                        case "invalid_email":
                            error = "El email que esta ingresando es incorrecto";
                            break;
                        case "already_in_team":
                            error = "Este email, ya se encuentra en nuestro Slack";
                            break;
                        default:
                            error = "Ocurrio un Error, Notifique a nuestro admin!";
                            break;
                    }

                    return StatusCode(409, error);
                }

                return Ok("Revise su email");
            }

            return BadRequest("El mail ingresado es invalido");

        }
    }
}
    