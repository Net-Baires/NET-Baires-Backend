using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Handlers.Sponsors
{
    public class NewSponsorCommand :  IRequest<IActionResult>
    {
        public IFormFile ImageFile { get; set; }
        public string Name { get; set; }
        public string SiteUrl { get; set; }
        public string Description { get; set; }
    }
}