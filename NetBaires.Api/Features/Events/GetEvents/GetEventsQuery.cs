using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.GetEvents
{
    public class GetEventsQuery : IRequest<IActionResult>
    {
        public bool? Done { get; set; }
        public bool? Live { get; set; }
        public bool? Upcoming { get; set; }
        public int? Id { get; set; }
        public int? MemberId { get; set; }

        public GetEventsQuery(bool? done, bool? live, int? id)
        {
            Done = done;
            Live = live;
            Id = id;
        }
        public GetEventsQuery(bool? done, bool? live, bool? upcoming)
        {
            Done = done;
            Live = live;
            Upcoming = upcoming;

        }
    }

}