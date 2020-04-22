using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetBaires.Events.DomainEvents;
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
            try
            {

                StreamReader reader = new StreamReader(myBlob);
                var removedByStringBuilder = new StringBuilder(reader.ReadToEnd());
                var message = new SendGridMessage();
                var data = JsonSerializer.Deserialize<AssignedBadgeToAttendance>(myQueueItem);

                log.LogInformation($"NotifyAssignedBadgeToAttendance - Started : MemberID : {data.MemberId}, BadgeId: {data.BadgeId}");


                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                var currentEnvironment = config["CurrentEnvironment"];
                var connectionString = config["ConnectionString"];
                var badgeLinkBuilder = new StringBuilder(config["BadgeLink"]);

                string memberName = string.Empty;
                using (var connection = new SqlConnection(connectionString))
                {
                    var member = connection.Query<Member>($"SELECT Email,FirstName,LastName FROM Members WHERE ID = {data.MemberId}").FirstOrDefault();
                    if (member == null || string.IsNullOrWhiteSpace(member.Email))
                        return;
                    memberName = string.Concat(member.FirstName, " ", member.LastName);
                    removedByStringBuilder.Replace("{{MemberName}}", memberName);
                    message.AddTo(member.Email);

                }
                using (var connection = new SqlConnection(connectionString))
                {
                    var badge = connection.Query<Badge>($"SELECT ImageUrl,Name,Description FROM Badges WHERE ID = {data.BadgeId}").FirstOrDefault();
                    removedByStringBuilder.Replace("{{BadgeName}}", badge.Name);
                    removedByStringBuilder.Replace("{{BadgeImageUrl}}", badge.ImageUrl);
                    message.SetSubject((new StringBuilder(config["NotifyAssignedBadgeToAttendanceSubject"])).Replace("{{MemberName}}", memberName)
                        .Replace("{{BadgeName}}", badge.Name)
                        .ToString());

                }

                badgeLinkBuilder.Replace("{{MemberId}}", data.MemberId.ToString());
                badgeLinkBuilder.Replace("{{BadgeId}}", data.BadgeId.ToString());


                removedByStringBuilder.Replace("{{BadgeLink}}", string.Concat(currentEnvironment, badgeLinkBuilder.ToString()));
                message.AddContent("text/html", removedByStringBuilder.ToString());
                message.SetFrom(config["Emails:From"]);

                await messageCollector.AddAsync(message);
                log.LogInformation($"NotifyAssignedBadgeToAttendance - Finished : MemberID : {data.MemberId}, BadgeId: {data.BadgeId}");
            }
            catch (Exception e)
            {
                log.LogError("NotifyAssignedBadgeToAttendance Error", e);
                throw;
            }

        }




        [FunctionName("NotifyToThankSponsors")]
        public static async Task NotifyToThankSponsors([QueueTrigger(nameof(NotifiedSponsorsEventEnd), Connection = "")]string myQueueItem,
         [Blob("templates/emails/NotifyToThankSponsors.html", FileAccess.Read)] Stream myBlob,
         ILogger log,
         [SendGrid(ApiKey = "CustomSendGridKeyAppSettingName")] IAsyncCollector<SendGridMessage> messageCollector,
         ExecutionContext context)
        {
            try
            {

                StreamReader reader = new StreamReader(myBlob);
                var removedByStringBuilder = new StringBuilder(reader.ReadToEnd());
                var message = new SendGridMessage();
                var data = JsonSerializer.Deserialize<NotifiedSponsorsEventEnd>(myQueueItem);

                log.LogInformation($"NotifyToThankSponsors - Started : EventId : {data.EventId}");


                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                var currentEnvironment = config["CurrentEnvironment"];
                var connectionString = config["ConnectionString"];
                var eventLinkBuilder = new StringBuilder(config["EventLink"]);
                using (var connection = new SqlConnection(connectionString))
                {
                    var sponsor = connection.Query<Sponsor>(@$"Select [Name],Email FROM Sponsors S INNER JOIN SponsorEvents SE
                                                                on S.Id = SE.SponsorId
                                                                INNER JOIN Events E ON E.Id = SE.EventId
                                                                WHERE E.Id = {data.EventId}").FirstOrDefault();
                    var @event = connection.Query<Event>($"SELECT Id,Title,Description FROM Events where Id = {data.EventId}").FirstOrDefault();
                    removedByStringBuilder.Replace("{{EventTitle}}", @event.Title);
                    removedByStringBuilder.Replace("{{SponsorName}}", sponsor.Name);
                    removedByStringBuilder.Replace("{{EventDescription}}", @event.Description);
                    eventLinkBuilder.Replace("{{EventId}}", @event.Id.ToString());
                    removedByStringBuilder.Replace("{{EventLink}}", string.Concat(currentEnvironment, eventLinkBuilder.ToString()));

                    message.AddContent("text/html", removedByStringBuilder.ToString());
                    message.SetFrom(config["EmailFrom"]);
                    await messageCollector.AddAsync(message);
                }

                log.LogInformation($"NotifyToThankSponsors - Finished : EventId : {data.EventId}");
            }
            catch (Exception e)
            {
                log.LogError("NotifyToThankSponsors Error", e);
                throw;
            }

        }

        public class Sponsor
        {
            public string Email { get; set; }
            public string Name { get; set; }
        }
        public class Event
        {
            public int Id { get; set; }
            public string Title{ get; set; }
            public string Description { get; set; }
        }
        public class Member
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        public class Badge
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
