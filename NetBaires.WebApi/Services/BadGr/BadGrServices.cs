using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Models.ServicesResponse.Badgr;
using NetBaires.Api.Options;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.BadGr
{
    public class BadGrServices : IBadGrServices
    {
        private readonly BadgrOptions _badgrOptions;
        private readonly ILogger<BadGrServices> _logger;
        private readonly HttpClient _client;

        public BadGrServices(IHttpClientFactory httpClientFactory,
            IOptions<BadgrOptions> badgrOptions,
            ILogger<BadGrServices> logger)
        {
            _badgrOptions = badgrOptions.Value;
            _logger = logger;
            _client = httpClientFactory.CreateClient();
        }
        public async Task<BadgrResponse<BadgClass>> GetAllBadget()
        {
            var req = new HttpRequestMessage(HttpMethod.Get,
                $"{_badgrOptions.EndPoint}/issuers/{_badgrOptions.Issuer}/badgeclasses");
            req.Headers.Add("Accept", "application/json");
            req.Headers.Add("Authorization", $"Bearer {_badgrOptions.Token}");
            var result = await _client.SendAsync(req);
            var slackResponse = await result.Content.ReadAsAsync<BadgrResponse<BadgClass>>();
            return slackResponse;
        }
        public async Task<BadgrResponse<BadgClass>> CreateAssertion(string badgeId, string email)
        {
            var assertionToUser = new AssertionToUser
            {
                Badgeclass = badgeId,
                EntityType = "Assertion",
                Recipient = new Recipient
                {
                    Hashed = true,
                    Identity = email,
                    Type = "email"
                }
            };

            var requestMessage = new HttpRequestMessage()
            {
                Method = new HttpMethod("POST"),
                RequestUri = new Uri($"{_badgrOptions.EndPoint}/issuers/{_badgrOptions.Issuer}/assertions"),
                Content =
                    new StringContent(JsonConvert.SerializeObject(assertionToUser), Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.Headers.Add("Authorization", $"Bearer {_badgrOptions.Token}");
            var result = await _client.SendAsync(requestMessage);


            var rea = await result.Content.ReadAsStringAsync();
            var slackResponse = await result.Content.ReadAsAsync<BadgrResponse<BadgClass>>();
            return slackResponse;
        }
    }
}
