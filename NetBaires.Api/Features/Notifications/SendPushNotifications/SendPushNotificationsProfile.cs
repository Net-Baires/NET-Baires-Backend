using AutoMapper;
using NetBaires.Api.Features.Sponsors.NewSponsor;
using NetBaires.Data;

namespace NetBaires.Api.Features.Notifications.SendPushNotifications
{
    public class SendPushNotificationsProfile : Profile
    {
        public SendPushNotificationsProfile()
        {
            CreateMap<SendPushNotificationsCommand, PushNotification>();
        }
    }
}