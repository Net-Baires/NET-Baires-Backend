using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Events.UpdateEvent
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
        public bool? Online { get; set; }
        public string OnlineLink { get; set; }
        public bool? GeneralAttended { get; set; }
        public List<SponsorEventViewModel> Sponsors { get; set; }
       
    }
    public class SponsorEventViewModel
    {
        public int SponsorId { get; set; }
        public string Detail { get; set; }
    }
}