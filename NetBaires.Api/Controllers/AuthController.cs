using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Models.ServicesResponse;
using NetBaires.Api.Options;

namespace NetBaires.Api.Controllers
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly TelemetryClient _telemetryClient;

        public ExceptionActionFilter(
            IHostingEnvironment hostingEnvironment,
            TelemetryClient telemetryClient)
        {
            _hostingEnvironment = hostingEnvironment;
            _telemetryClient = telemetryClient;
        }

        #region Overrides of ExceptionFilterAttribute

        public override void OnException(ExceptionContext context)
        {
            var actionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            Type controllerType = actionDescriptor.ControllerTypeInfo;

            var controllerBase = typeof(ControllerBase);
            var controller = typeof(Controller);

            // Api's implements ControllerBase but not Controller
            if (controllerType.IsSubclassOf(controllerBase) && !controllerType.IsSubclassOf(controller))
            {
                // Handle web api exception
            }

            // Pages implements ControllerBase and Controller
            if (controllerType.IsSubclassOf(controllerBase) && controllerType.IsSubclassOf(controller))
            {
                // Handle page exception
            }

            if (!_hostingEnvironment.IsDevelopment())
            {
                // Report exception to insights
                _telemetryClient.TrackException(context.Exception);
                _telemetryClient.Flush();
            }

            base.OnException(context);
        }

        #endregion
    }
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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