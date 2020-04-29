using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NetBaires.Events.DomainEvents;
using NetBaires.Services.MakeEmail;
using SendGrid.Helpers.Mail;

namespace NetBaires.Services
{
    public static class Notifications
    {
        [FunctionName("NotifyAssignedBadgeToAttendance")]
        public static async Task NotifyAssignedBadgeToAttendance([QueueTrigger(nameof(AssignedBadgeToMember), Connection = "")]string myQueueItem,
            [Blob("templates/emails/NotifyAssignedBadge.html", FileAccess.Read)] Stream myBlob,
            ILogger log,
            [SendGrid(ApiKey = "CustomSendGridKeyAppSettingName")] IAsyncCollector<SendGridMessage> messageCollector,
            ExecutionContext context)
        {
            await (new NotifyEmails()).Notify(new NotifyAssignedBadgeToMemberMakeEmail(), myQueueItem, log, messageCollector, context, nameof(AssignedBadgeToMember));

        }


        [FunctionName("NotifiedSponsorsEventEnd")]
        public static async Task NotifyToThankSponsors([QueueTrigger(nameof(NotifiedSponsorsEventEnd), Connection = "")]string myQueueItem,
            ILogger log,
            [SendGrid(ApiKey = "CustomSendGridKeyAppSettingName")] IAsyncCollector<SendGridMessage> messageCollector,
            ExecutionContext context)
        {
            await (new NotifyEmails()).Notify(new NotifiedSponsorsEventEndMakeEmail(), myQueueItem, log, messageCollector, context, nameof(NotifiedSponsorsEventEnd));
        }


        [FunctionName("NotifiedSpeakersEventEnd")]
        public static async Task NotifiedSpeakersEventEnd([QueueTrigger(nameof(Events.DomainEvents.NotifiedSpeakersEventEnd), Connection = "")]string myQueueItem,
            ILogger log,
            [SendGrid(ApiKey = "CustomSendGridKeyAppSettingName")] IAsyncCollector<SendGridMessage> messageCollector,
            ExecutionContext context)
        {
            await (new NotifyEmails()).Notify(new NotifiedSpeakersEventEndMakeEmail(), myQueueItem, log, messageCollector, context, nameof(Events.DomainEvents.NotifiedSpeakersEventEnd));
        }
        [FunctionName("NotifiedAttendedEventEnd")]
        public static async Task NotifiedAttendedEventEnd([QueueTrigger(nameof(Events.DomainEvents.NotifiedAttendedEventEnd), Connection = "")]string myQueueItem,
            ILogger log,
            [SendGrid(ApiKey = "CustomSendGridKeyAppSettingName")] IAsyncCollector<SendGridMessage> messageCollector,
            ExecutionContext context)
        {
            await (new NotifyEmails()).Notify(new NotifiedAttendedEventEndMakeEmail(), myQueueItem, log, messageCollector, context, nameof(Events.DomainEvents.NotifiedAttendedEventEnd));
        }

    }
}
