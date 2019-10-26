using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using NetBaires.Api.Auth;
using MediatR;
using NetBaires.Api.Handlers.Badges;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SponsorsController : ControllerBase
    {
        private readonly ILogger<SlackController> _logger;
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly IMediator mediator;

        public SponsorsController(NetBairesContext context,
            ICurrentUser currentUser,
            IMediator mediator,
            ILogger<SlackController> logger)
        {
            _logger = logger;
            _context = context;
            _currentUser = currentUser;
            this.mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> Get() =>
        await mediator.Send(new GetSponsorsHandler.GetSponsors());

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> GetById(int id)
        {
            var command = new GetSponsorHandler.GetSponsor(id);
            return await mediator.Send(command);
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Post([FromForm]NewSponsorHandler.NewSponsor sponsor) =>
             await mediator.Send(sponsor);

        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Put(int id, [FromForm]UpdateSponsorHandler.UpdateSponsor sponsor)
        {
            sponsor.Id = id;
            return await mediator.Send(sponsor);
        }

        [HttpDelete("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var sponsorToDelete = await _context.Sponsors.FirstOrDefaultAsync(x => x.Id == id);
            if (sponsorToDelete == null)
                return NotFound();
            _context.Sponsors.Remove(sponsorToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}