using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Options;
using NetBaires.Api.Services.Meetup.Models;

namespace NetBaires.Api.Services.Meetup
{
    public class MeetupServices : IMeetupServices
    {
        private readonly MeetupEndPointOptions _meetupEndPointOptions;
        private readonly ILogger<MeetupServices> _logger;
        private readonly HttpClient _client;

        public MeetupServices(IHttpClientFactory httpClientFactory,
            IOptions<MeetupEndPointOptions> meetupEndPointOptions,
            ILogger<MeetupServices> logger)
        {
            _meetupEndPointOptions = meetupEndPointOptions.Value;
            _logger = logger;
            _client = httpClientFactory.CreateClient();
        }
        public async Task<List<MeetupEventDetail>> GetAllEvents()
        {
            var req = new HttpRequestMessage(HttpMethod.Get,
                $"{_meetupEndPointOptions.Url}/net-baires/events?&sign=true&photo-host=public&status=past,upcoming&fields=featured_photo");
            req.Headers.Add("Accept", "application/json");
            req.Headers.Add("Authorization", $"Bearer {_meetupEndPointOptions.Token}");

            var result = await _client.SendAsync(req);
            var events = await result.Content.ReadAsAsync<List<MeetupEventDetail>>();
            return events;
        }
        public async Task<List<AttendanceResponse>> GetAttendees(int eventId)
        {
            var req = new HttpRequestMessage(HttpMethod.Get,
                $"{_meetupEndPointOptions.Url}/net-baires/events/{eventId}/attendance?&sign=true&photo-host=public");
            req.Headers.Add("Accept", "application/json");
            req.Headers.Add("Authorization", $"Bearer {_meetupEndPointOptions.Token}");
            var result = await _client.SendAsync(req);
            if (!result.IsSuccessStatusCode)
                return new List<AttendanceResponse>();

            var attendanceResponse = await result.Content.ReadAsAsync<List<AttendanceResponse>>();
            if (attendanceResponse == null)
                return new List<AttendanceResponse>();
            return attendanceResponse;
        }
    }
}