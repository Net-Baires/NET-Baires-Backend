using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Sponsors.UpdateSponsor
{
    public class UpdateSponsorCommand : IRequest<IActionResult>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SiteUrl { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}