using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Notifications.SendPushNotifications
{
    public class SendPushNotificationsCommand :  IRequest<IActionResult>
    {
        public List<string> PushNotificationIds { get; set; }
        public Notification Notification { get; set; }
        public Data Data { get; set; }
        public FcmOptions FcmOptions { get; set; }
    }
}