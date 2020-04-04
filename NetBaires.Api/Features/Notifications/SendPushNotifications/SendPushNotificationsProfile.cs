using AutoMapper;

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