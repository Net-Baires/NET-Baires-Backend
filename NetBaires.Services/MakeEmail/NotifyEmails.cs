using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace NetBaires.Services.MakeEmail
{
    public class NotifyEmails
    {
        public async Task Notify<TData>(IMakeEmail<TData> makeBody, string myQueueItem, Stream myBlob, ILogger log, IAsyncCollector<SendGridMessage> messageCollector,
            ExecutionContext context, string notificationName)
        {
            try
            {
                log.LogInformation($"{notificationName} - Started");

                StreamReader reader = new StreamReader(myBlob);
                
                var data = JsonSerializer.Deserialize<TData>(myQueueItem);
                foreach (var email in makeBody.Make(data, reader, new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build()))
                {
                    var message = new SendGridMessage();
                    message.AddContent("text/html", email.Body);
                    message.SetFrom(email.Email);
                    message.SetSubject(email.Subject);
                    await messageCollector.AddAsync(message);
                }

                log.LogInformation($"{notificationName} - Finished");
            }
            catch (Exception e)
            {
                log.LogError($"{notificationName} Error", e);
                throw;
            }
        }

    }
}