using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NetBaires.Api.Features.Members.SearchMember
{
    public class UpdateInformationCommand : IRequest<IActionResult>
    {
        public string PushNotificationId { get; set; }
    }
}