using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetBaires.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using NetBaires.Api.Auth;

namespace NetBaires.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorsController : ControllerBase
    {
        private readonly ILogger<SlackController> _logger;
        private readonly NetBairesContext _context;
        private readonly ICurrentUser _currentUser;

        public SponsorsController(NetBairesContext context,
            ICurrentUser currentUser,
            ILogger<SlackController> logger)
        {
            _logger = logger;
            _context = context;
            _currentUser = currentUser;
        }

        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> Get()
        {
            var sponsors = _context.Sponsors.AsNoTracking();
            if (sponsors.Any())
                return Ok(sponsors);
            return NoContent();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ApiExplorerSettingsExtend(UserAnonymous.Anonymous)]
        public async Task<IActionResult> GetById(int id)
        {
            var sponsors = await _context.Sponsors.FirstOrDefaultAsync(x => x.Id == id);
            if (sponsors == null)
                return NotFound();

            return Ok(sponsors);
        }

        [HttpPost]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Post([FromBody]Sponsor sponsor)
        {
            var email = _currentUser.User.Email;
            if (_context.Sponsors.Any(x => x.Name.ToUpper() == sponsor.Name.ToUpper()))
                return BadRequest("El nombre de sponsor que esta queriando utilizar, ya se encuentra registrado");

            await _context.Sponsors.AddAsync(sponsor);
            await _context.SaveChangesAsync();
            return Ok(sponsor);
        }
        [HttpPut("{id}")]
        [AuthorizeRoles(UserRole.Admin)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> Put(int id, Sponsor sponsor)
        {
            sponsor.Id = id;

            _context.Entry(sponsor).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(sponsor);
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