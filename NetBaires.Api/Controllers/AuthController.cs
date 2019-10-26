using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Models.ServicesResponse;
using NetBaires.Api.Options;
using NetBaires.Data;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private readonly IOptions<MeetupEndPointOptions> _meetupEndPointOptions;
        private readonly HttpClient _client;

        public AuthController(IUserService userService,
            IOptions<MeetupEndPointOptions> meetupEndPointOptions,
            IHttpClientFactory httpClientFactory)
        {
            _userService = userService;
            _meetupEndPointOptions = meetupEndPointOptions;
            _client = httpClientFactory.CreateClient();
        }
        [AllowAnonymous]
        [HttpPost("Meetup")]
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
                var user = await _userService.AuthenticateOrCreate(response.email);
                return Ok(user);
            }

            return BadRequest();
        }


        [AllowAnonymous]
        [HttpPost("EventBrite")]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public IActionResult EventBrite([FromBody]AuthenticateModel model)
        {
            var email = "german.kuber@outlook.com";

            var user = _userService.AuthenticateOrCreate(email);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}