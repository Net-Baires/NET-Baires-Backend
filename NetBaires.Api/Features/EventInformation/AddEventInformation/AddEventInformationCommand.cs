using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.EventInformation.AddEventInformation
{
    public class AddEventInformationCommand : IRequest<IActionResult>
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; }
    }
}