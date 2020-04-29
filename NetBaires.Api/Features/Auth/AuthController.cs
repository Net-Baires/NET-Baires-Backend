using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Features.Auth.AuthEventBrite;
using NetBaires.Api.Models.ServicesResponse;
using NetBaires.Api.Options;
using NetBaires.Data;
using NetBaires.Data.Entities;
using PusherServer;

namespace NetBaires.Api.Features.Auth
{
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private readonly IOptions<MeetupEndPointOptions> _meetupEndPointOptions;
        private readonly IMediator _mediator;
        private readonly HttpClient _client;

        public AuthController(IUserService userService,
            IOptions<MeetupEndPointOptions> meetupEndPointOptions,

            IMediator mediator,
            IHttpClientFactory httpClientFactory)
        {
            _userService = userService;
            _meetupEndPointOptions = meetupEndPointOptions;
            _mediator = mediator;
            _client = httpClientFactory.CreateClient();
        }
        [AllowAnonymous]
        [HttpPost("Auth/Meetup")]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> Meetup([FromBody]AuthenticateModel model)
        {
            var dict = new Dictionary<string, string>
            {
                {"client_id", _meetupEndPointOptions.Value.ClientId},
                {"client_secret", _meetupEndPointOptions.Value.ClientSecret},
                {"grant_type", "refresh_token"},
                {"refresh_token", model.Token}
            };
            var req = new HttpRequestMessage(HttpMethod.Get, "https://api.meetup.com/members/self")
            {
                Content = new FormUrlEncodedContent(dict)
            };
            req.Headers.Add("Authorization", $"Bearer {model.Token}");
            var result = await _client.SendAsync(req);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsAsync<MeetupSelf>();
                var user = await _userService.AuthenticateOrCreate(response.email, response.id);

                return Ok(user);
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("Pusher/Auth")]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> Pusher([FromForm]string channel_name, [FromForm]string socket_id)
        {
            var pusher = new Pusher("924297", "b764a51bdf1cdf760ad6", "9250a463d6d5efea534d", new PusherOptions
            {
                Cluster = "us2"
            });
            var auth = pusher.Authenticate(channel_name, socket_id, new PresenceChannelData()).ToJson();
            return new ContentResult { Content = auth, ContentType = "application/json" };
        }


        [AllowAnonymous]
        [HttpPost("Auth/EventBrite")]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> EventBrite([FromBody]AuthEventBriteCommand command) =>
            await _mediator.Send(command);


    }
}