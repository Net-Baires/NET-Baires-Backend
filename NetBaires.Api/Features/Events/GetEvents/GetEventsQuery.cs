using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.GetEvents
{
    public class GetEventsQuery : IRequest<IActionResult>
    {
        public bool? Done { get; set; }
        public bool? Live { get; set; }
        public int? Id { get; set; }

        public GetEventsQuery(bool? done, bool? live, int? id)
        {
            Done = done;
            Live = live;
            Id = id;
        }
        public GetEventsQuery(bool? done, bool? live)
        {
            Done = done;
            Live = live;
        }
    }

}