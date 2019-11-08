using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBaires.Api.Options;
using NetBaires.Api.Services.EventBrite.Models;

namespace NetBaires.Api.Services.EventBrite
{
    public interface IEventBriteServices
    {
        Task<List<Attendee>> GetAttendees(string eventId);
        Task<List<Event>> GetEvents();
    }
    public class EventBriteServices : IEventBriteServices
    {
        private readonly EventBriteApiOptions _eventBriteApiOptions;
        private HttpClient _client;

        public EventBriteServices(IOptions<EventBriteApiOptions> eventBriteApiOptions,
            IHttpClientFactory httpClientFactory,
            ILogger<EventBriteServices> logger)
        {
            _eventBriteApiOptions = eventBriteApiOptions.Value;
            _client = httpClientFactory.CreateClient("");
        }

        public async Task<List<Attendee>> GetAttendees(string eventId)
        {
            var req = new HttpRequestMessage(HttpMethod.Get,
                $"{_eventBriteApiOptions.Url}/events/${eventId}/attendees?token=${_eventBriteApiOptions.Token}");
            req.Headers.Add("Accept", "application/json");
            var result = await _client.SendAsync(req);
            var attendeesResponse = await result.Content.ReadAsAsync<AttendeesResponse>();
            if (attendeesResponse.Attendees == null)
                return new List<Attendee>();
            return attendeesResponse.Attendees?.ToList();
        }
        public async Task<List<Event>> GetEvents()
        {
            var req = new HttpRequestMessage(HttpMethod.Get,
                $"{_eventBriteApiOptions.Url}/users/me/events?token={_eventBriteApiOptions.Token}");
            req.Headers.Add("Accept", "application/json");
            var result = await _client.SendAsync(req);
            var attendeesResponse = await result.Content.ReadAsAsync<EventsResponse>();
            if (attendeesResponse.Events == null)
                return new List<Event>();
            return attendeesResponse.Events?.ToList();
        }
    }
}
