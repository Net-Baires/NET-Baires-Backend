using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Flurl;
using Flurl.Http;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Helpers;
using NetBaires.Api.Services;
using NetBaires.Api.ViewModels;
using NetBaires.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NetBaires.Api.Features.Notifications.SendPushNotifications
{
    public class SendPushNotificationsHandler : IRequestHandler<SendPushNotificationsCommand, IActionResult>
    {
        private readonly IMapper _mapper;

        public SendPushNotificationsHandler(IMapper mapper)
        {
            _mapper = mapper;
        }


        public async Task<IActionResult> Handle(SendPushNotificationsCommand request, CancellationToken cancellationToken)
        {
            var pushNotificationToSend = _mapper.Map<PushNotification>(request);

            foreach (var notification in request.PushNotificationIds)
            {
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                pushNotificationToSend.To = notification;
                var pushNotification = await "https://fcm.googleapis.com/fcm".AppendPathSegment("send")
                    .WithHeader("Authorization", "key=AAAAp6mIFl8:APA91bGoewvoSfIbLcHFL4OCxPa19drL-pWuQ5YYpsGh3gh_xdWLyZTS2F3pwpUJpMPWPiIpkJTq0oPbIomCCpG3zFTEutxDJGJT703uebKnTCFnYe9GcqzIuFURQwYueOIW6eINonjY")
                    .PostAsync(new StringContent(JsonConvert.SerializeObject(pushNotificationToSend, new JsonSerializerSettings
                    {
                        ContractResolver = contractResolver,
                        Formatting = Formatting.Indented
                    }), Encoding.UTF8, "application/json"))
                    .ReceiveJson();
            }
           
            return HttpResponseCodeHelper.NotContent();
        }
    }

    public class PushNotification
    {
        public string To { get; set; }
        public Notification Notification { get; set; }
        public Data Data { get; set; }
        [JsonProperty(PropertyName = "fcm_options")]
        public FcmOptions FcmOptions { get; set; }

        public PushNotification(string to, Notification notification, Data data, FcmOptions fcmOptions)
        {
            To = to;
            Notification = notification;
            Data = data;
            FcmOptions = fcmOptions;
        }

        public PushNotification()
        {
            
        }
    }

    public class Notification
    {
        public string Title { get; set; }
        public string Body { get; set; }
        [JsonProperty(PropertyName = "mutable_content")]
        public bool MutableContent { get; set; }
        public string Sound { get; set; }
        public string Color { get; set; }
        public bool Sticky { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }

        public Notification(string title, string body,string icon, string image)
        {
            Title = title;
            Body = body;
            Icon = icon;
            Image = image;
        }

        public Notification()
        {
            
        }
    }

    public class Data
    {
        public string Url { get; set; }
        public string Dl { get; set; }
    }

    public class FcmOptions
    {
        public string Link { get; set; }
    }


}