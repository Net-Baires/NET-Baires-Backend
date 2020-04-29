using System;
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
        public async Task Notify<TData>(IMakeEmail<TData> makeBody, string myQueueItem, ILogger log,
            IAsyncCollector<SendGridMessage> messageCollector,
            ExecutionContext context, string notificationName)
        {
            try
            {
                log.LogInformation($"{notificationName} - Started");
                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", true, true)
                    .AddEnvironmentVariables()
                    .Build();
                var emailFrom = config["EmailFrom"];
                var data = JsonSerializer.Deserialize<TData>(myQueueItem);
                foreach (var email in makeBody.Make(data, config))
                {
                    var message = new SendGridMessage();
                    message.AddContent("text/html", email.Body);
                    message.SetFrom(emailFrom);
                    message.AddTo(email.Email);
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