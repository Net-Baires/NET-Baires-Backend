using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.EventInformation.UpdateEventInformation
{
    public class UpdateEventInformationCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; }
        
    }
}