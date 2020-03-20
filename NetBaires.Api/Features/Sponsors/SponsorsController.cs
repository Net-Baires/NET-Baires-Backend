using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.Sponsors.DeleteSponsor;
using NetBaires.Api.Features.Sponsors.GetSponsors;
using NetBaires.Api.Features.Sponsors.NewSponsor;
using NetBaires.Api.Features.Sponsors.UpdateSponsor;
using NetBaires.Data;

namespace NetBaires.Api.Features.Sponsors
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SponsorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SponsorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> GetSponsors() =>
        await _mediator.Send(new GetSponsorsQuery());

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> GetById([FromRoute]GetSponsorsQuery query) =>
            await _mediator.Send(query);

        [HttpPost]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> NewSponsor([FromForm]NewSponsorCommand command) =>
             await _mediator.Send(command);

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> UpdateSponsor([FromForm]UpdateSponsorCommand command, [FromRoute]int id)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> DeleteSponsor([FromRoute]DeleteSponsorCommand command) =>
            await _mediator.Send(command);
    }
}