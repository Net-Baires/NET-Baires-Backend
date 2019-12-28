using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NetBaires.Api.Handlers.Events
{
    public class UpdateEventCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public bool? Live { get; set; }
        public bool? Done { get; set; }
        public bool? GeneralAttended { get; set; }
        public List<SponsorEvent> Sponsors { get; set; }
        public class SponsorEvent {
            public int SponsorId { get; set; }
            public string Detail { get; set; }
        }
    }
}