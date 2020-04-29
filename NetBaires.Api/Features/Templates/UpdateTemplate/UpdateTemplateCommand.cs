using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Templates.UpdateTemplate
{
    public class UpdateTemplateCommand : IRequest<IActionResult>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TemplateContent { get; set; }
    }
}