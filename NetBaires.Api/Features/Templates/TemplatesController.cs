using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.Templates.DeleteTemplate;
using NetBaires.Api.Features.Templates.GetTemplates;
using NetBaires.Api.Features.Templates.NewTemplate;
using NetBaires.Api.Features.Templates.UpdateTemplate;
using NetBaires.Api.ViewModels;
using NetBaires.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.Templates
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TemplatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TemplatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [ProducesResponseType(typeof(TemplateViewModel), 200)]
        public async Task<IActionResult> GetTemplates() =>
        await _mediator.Send(new GetTemplatesQuery());

        [HttpGet("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [ProducesResponseType(typeof(TemplateViewModel), 200)]
        public async Task<IActionResult> GetTemplateById([FromRoute]GetTemplatesQuery query) =>
            await _mediator.Send(query);

        [HttpPost]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [ProducesResponseType(typeof(TemplateViewModel), 200)]
        public async Task<IActionResult> NewTemplate([FromBody]NewTemplateCommand command) =>
             await _mediator.Send(command);

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        [ProducesResponseType(typeof(TemplateViewModel), 200)]
        public async Task<IActionResult> UpdateTemplate([FromBody]UpdateTemplateCommand command, [FromRoute]int id)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> DeleteTemplate([FromRoute]DeleteTemplateCommand command) =>
            await _mediator.Send(command);
    }
}