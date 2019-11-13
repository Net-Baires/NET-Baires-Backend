using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Events
{
    public class GetEventsQuery : IRequest<IActionResult>
    {
        public bool? Done { get; set; }
        public bool? Live { get; set; }
    }

}