using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Helpers;
using NetBaires.Api.Services;
using NetBaires.Api.ViewModels;
using NetBaires.Data;

namespace NetBaires.Api.Features.Notifications.SendPushNotifications
{
    public class SendPushNotificationsHandler : IRequestHandler<SendPushNotificationsCommand, IActionResult>
    {

        public SendPushNotificationsHandler()
        {
        }


        public async Task<IActionResult> Handle(SendPushNotificationsCommand request, CancellationToken cancellationToken)
        {
          
            return HttpResponseCodeHelper.Ok();
        }
    }
}