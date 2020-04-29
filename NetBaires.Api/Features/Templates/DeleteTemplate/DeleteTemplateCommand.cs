using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Templates.DeleteTemplate
{
    public class DeleteTemplateCommand :  IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
}