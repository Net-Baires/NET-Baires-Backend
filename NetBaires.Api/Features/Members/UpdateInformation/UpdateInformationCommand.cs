using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Members.UpdateInformation
{
    public class UpdateInformationCommand : IRequest<IActionResult>
    {
        public string PushNotificationId { get; set; }
    }
}