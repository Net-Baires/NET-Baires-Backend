using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NetBaires.Events.DomainEvents;

namespace NetBaires.Services.MakeEmail
{
    public class NotifiedSponsorsEventEndMakeEmail : IMakeEmail<NotifiedSponsorsEventEnd>
    {

        public List<EmailToSend> Make(NotifiedSponsorsEventEnd data,  IConfigurationRoot config)
        {
            var returnList = new List<EmailToSend>();
            var currentEnvironment = config["CurrentEnvironment"];
            var connectionString = config["ConnectionString"];
            var eventLinkBuilder = new StringBuilder(config["EventLink"]);

            using (var connection = new SqlConnection(connectionString))
            {
                var sponsors = connection.Query<Sponsor>(@$"Select [Name],Email FROM Sponsors S INNER JOIN SponsorEvents SE
                                                                on S.Id = SE.SponsorId
                                                                INNER JOIN Events E ON E.Id = SE.EventId
                                                                WHERE E.Id = {data.EventId}").ToList();
                if (!sponsors.Any())
                    throw new EventDoesNotHaveSponsors(data.EventId);
                var @event = connection.Query<Event>($"SELECT Id,Title,Description,EmailTemplateThanksSponsorsId FROM Events where Id = {data.EventId}")
                    .FirstOrDefault();
                eventLinkBuilder.Replace("{{EventId}}", @event.Id.ToString());
                var template = connection
                    .Query<Template>($"SELECT TemplateContent FROM Templates Where Id = {@event.EmailTemplateThanksSponsorsId}")
                    .FirstOrDefault();
                foreach (var sponsor in sponsors)
                {
                    var builder = new StringBuilder(template.TemplateContent);
                    builder.Replace("{{EventTitle}}", @event.Title);
                    builder.Replace("{{SponsorName}}", sponsor.Name);
                    builder.Replace("{{EventDescription}}", @event.Description);
                    builder.Replace("{{EventLink}}",
                        string.Concat(currentEnvironment, eventLinkBuilder.ToString()));
                    returnList.Add(new EmailToSend(sponsor.Email, builder.ToString(), GetSubject(config, sponsor.Name)));
                }
            }

            return returnList;
        }

        private string GetSubject(IConfigurationRoot config, string sponsorName)
        {
            return new StringBuilder(config["NotifiedSponsorsEventEndSubject"])
                .Replace("{{SponsorTitle}}", sponsorName)
                .ToString();
        }

        public class Sponsor
        {
            public string Email { get; set; }
            public string Name { get; set; }
        }
        public class Event
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int EmailTemplateThanksSponsorsId { get; set; }
        }


    }
}