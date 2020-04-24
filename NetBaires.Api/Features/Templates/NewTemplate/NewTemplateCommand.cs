using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Features.Templates.NewTemplate
{
    public class NewTemplateCommand :  IRequest<IActionResult>
    {
        public TemplateTypeEnum Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TemplateContent { get; set; }
    }
}