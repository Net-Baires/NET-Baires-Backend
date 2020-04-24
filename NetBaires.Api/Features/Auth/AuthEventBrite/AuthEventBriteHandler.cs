using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EFSecondLevelCache.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetBaires.Api.Auth;
using NetBaires.Api.Helpers;
using NetBaires.Api.Options;
using NetBaires.Api.ViewModels;
using Newtonsoft.Json;

namespace NetBaires.Api.Features.Auth.AuthEventBrite
{
    public class EventBriteToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
    public partial class EventBriteMe
    {
        [JsonProperty("emails")]
        public Email[] Emails { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("image_id")]
        public object ImageId { get; set; }
    }

    public partial class Email
    {
        [JsonProperty("email")]
        public string EmailEmail { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }
    }

    public class AuthEventBriteHandler : IRequestHandler<AuthEventBriteCommand, IActionResult>
    {
        private readonly IUserService _userService;
        private readonly IOptionsMonitor<EventBriteApiOptions> _eventMonitor;
        private readonly IMapper _mapper;
        private HttpClient _client;

        public AuthEventBriteHandler(IUserService userService,
        IHttpClientFactory httpClientFactory,
            IOptionsMonitor<EventBriteApiOptions> eventMonitor,
            IMapper mapper)
        {
            _userService = userService;
            _eventMonitor = eventMonitor;
            _mapper = mapper;
            _client = httpClientFactory.CreateClient();
        }


        public async Task<IActionResult> Handle(AuthEventBriteCommand request, CancellationToken cancellationToken)
        {
            var dict = new Dictionary<string, string>
            {
                {"client_id", _eventMonitor.CurrentValue.ClientId},
                {"client_secret", _eventMonitor.CurrentValue.ClientSecret},
                {"grant_type", "authorization_code"},
                {"code", request.Token},
                {"redirect_uri", _eventMonitor.CurrentValue.RedirectUrl},
            };
            var req = new HttpRequestMessage(HttpMethod.Post, _eventMonitor.CurrentValue.UrlToken)
            {
                Content = new FormUrlEncodedContent(dict)
            };
            var result = await _client.SendAsync(req, cancellationToken);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsAsync<EventBriteToken>();
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {response.AccessToken}"); 
                var res = await _client.GetAsync($"{_eventMonitor.CurrentValue.Url}/users/me/?expand=assortment");


                if (res.IsSuccessStatusCode)
                {
                    var value = await res.Content.ReadAsAsync<EventBriteMe>();
                    var user = await _userService.AuthenticateOrCreateEventbrite(value);
                    return HttpResponseCodeHelper.Ok(user);
                }

            }

            return HttpResponseCodeHelper.Error();
        }
    }
}