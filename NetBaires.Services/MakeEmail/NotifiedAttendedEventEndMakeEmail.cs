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


    public class NotifiedAttendedEventEndMakeEmail : IMakeEmail<NotifiedAttendedEventEnd>
    {

        public List<EmailToSend> Make(NotifiedAttendedEventEnd data, IConfigurationRoot config)
        {
            var returnList = new List<EmailToSend>();
            var currentEnvironment = config["CurrentEnvironment"];
            var connectionString = config["ConnectionString"];
            using (var connection = new SqlConnection(connectionString))
            {


                var @event = connection.Query<Event>($"SELECT Title, EmailTemplateThanksAttendedId FROM Events WHERE Id = {data.EventId}").FirstOrDefault();
                var member = connection.Query<Member>($"SELECT Email, FirstName, LastName FROM Members WHERE Id = {data.MemberId}").FirstOrDefault();

                var template = connection
                    .Query<Template>($"SELECT TemplateContent FROM Templates Where Id = {@event.EmailTemplateThanksAttendedId}")
                    .FirstOrDefault();

                var memberProfileBuilder = new StringBuilder(config["MemberProfileLink"]);
                var memberName = $"{member.FirstName} {member.LastName}";

                var builder = new StringBuilder(template.TemplateContent);

                if (data.SendMaterialToAttendee)
                {
                    var material = connection
                        .Query<Material>($@"SELECT Link,Title FROM Materials WHERE EventId = {data.EventId}").ToList();

                    var materialBuilder = new StringBuilder();
                    foreach (var mat in material)
                    {
                        materialBuilder.Append("<li><a href='{{Link}}' target='_blank'>{{Title}}</a></li>");
                        materialBuilder.Replace("{{Link}}", mat.Link);
                        materialBuilder.Replace("{{Title}}", mat.Title);
                    }
                    builder.Replace("{{Material}}", materialBuilder.ToString());
                }
                else
                    builder.Replace("{{Material}}", string.Empty);

                builder.Replace("{{EventTitle}}", @event.Title);
                builder.Replace("{{MemberName}}", memberName);
                memberProfileBuilder.Replace("{{MemberId}}", data.MemberId.ToString());
                builder.Replace("{{MemberProfileLink}}",
                    string.Concat(currentEnvironment, memberProfileBuilder.ToString()));
                returnList.Add(new EmailToSend(member.Email, builder.ToString(), GetSubject(config, memberName)));

            }

            return returnList;
        }

        public string GetSubject(IConfigurationRoot config, string speakerName) =>
            new StringBuilder(config["NotifiedSpeakersSubject"])
                .Replace("{{SpeakerName}}", speakerName)
                .ToString();


        public class Material
        {
            public string Title { get; set; }
            public string Link { get; set; }
        }

        public class Event
        {
            public string Title { get; set; }
            public int EmailTemplateThanksAttendedId { get; set; }
        }
        public class Member
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }

    public class Template
    {
        public string TemplateContent { get; set; }
    }
}