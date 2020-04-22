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
        public static async Task NotifyAssignedBadgeToAttendance([QueueTrigger(nameof(AssignedBadgeToAttendance), Connection = "")]string myQueueItem,
            [Blob("templates/emails/NotifyAssignedBadge.html", FileAccess.Read)] Stream myBlob,
            ILogger log,
            [SendGrid(ApiKey = "CustomSendGridKeyAppSettingName")] IAsyncCollector<SendGridMessage> messageCollector,
            ExecutionContext context)
        {
            await (new NotifyEmails()).Notify(new NotifyAssignedBadgeToAttendanceMakeEmail(), myQueueItem, myBlob, log, messageCollector, context, nameof(AssignedBadgeToAttendance));

        }


        [FunctionName("NotifiedSponsorsEventEnd")]
        public static async Task NotifyToThankSponsors([QueueTrigger(nameof(NotifiedSponsorsEventEnd), Connection = "")]string myQueueItem,
            [Blob("templates/emails/NotifyToThankSponsors.html", FileAccess.Read)] Stream myBlob,
            ILogger log,
            [SendGrid(ApiKey = "CustomSendGridKeyAppSettingName")] IAsyncCollector<SendGridMessage> messageCollector,
            ExecutionContext context)
        {
            await (new NotifyEmails()).Notify(new NotifiedSponsorsEventEndMakeEmail(), myQueueItem, myBlob, log, messageCollector, context, nameof(NotifiedSponsorsEventEnd));
        }


        [FunctionName("NotifiedSpeakersEventEnd")]
        public static async Task NotifiedSpeakersEventEnd([QueueTrigger(nameof(NotifiedSpeakersEventEnd), Connection = "")]string myQueueItem,
            [Blob("templates/emails/NotifyToThankSponsors.html", FileAccess.Read)] Stream myBlob,
            ILogger log,
            [SendGrid(ApiKey = "CustomSendGridKeyAppSettingName")] IAsyncCollector<SendGridMessage> messageCollector,
            ExecutionContext context)
        {
            await (new NotifyEmails()).Notify(new NotifiedSponsorsEventEndMakeEmail(), myQueueItem, myBlob, log, messageCollector, context, nameof(NotifiedSpeakersEventEnd));
        }



    }
}
