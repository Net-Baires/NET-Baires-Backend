using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Members.SearchMember
{
    public class UpdateInformationCommand : IRequest<IActionResult>
    {
        public string PushNotificationId { get; set; }
    }
}