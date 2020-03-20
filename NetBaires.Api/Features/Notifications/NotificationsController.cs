using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.Notifications.SendPushNotifications;
using NetBaires.Data;

namespace NetBaires.Api.Features.Notifications
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("push")]
        //[AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Admin)]
        public async Task<IActionResult> NewSponsor([FromBody]SendPushNotificationsCommand command) =>
             await _mediator.Send(command);


    }
}