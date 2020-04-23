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
    public class NotifiedSpeakersEventEndMakeEmail : IMakeEmail<NotifiedSpeakersEventEnd>
    {
        private List<Member> _members;

        public List<EmailToSend> Make(NotifiedSpeakersEventEnd data, StreamReader reader, IConfigurationRoot config)
        {
            var returnList = new List<EmailToSend>();
            var currentEnvironment = config["CurrentEnvironment"];
            var connectionString = config["ConnectionString"];
            using (var connection = new SqlConnection(connectionString))
            {
                _members = connection.Query<Member>(
                    @$"SELECT E.Title,M.Id, M.FirstName, M.LastName, M.Email FROM Attendances A
                                                            INNER JOIN Members M
                                                            ON A.MemberId = M.Id
                                                            INNER JOIN Events E
                                                            ON E.Id = A.EventId
                                                            WHERE E.Id = {data.EventId} AND Speaker = 1").ToList();
                var template = reader.ReadToEnd();
                foreach (var member in _members)
                {
                    var memberProfileBuilder = new StringBuilder(config["EventLink"]);
                    var speakerName = $"{member.FirstName} {member.LastName}";
                    var builder = new StringBuilder(template);
                    builder.Replace("{{EventTitle}}", member.Title);
                    builder.Replace("{{SpeakerName}}", speakerName);
                    memberProfileBuilder.Replace("{{MemberId}}", member.Id.ToString());
                    builder.Replace("{{MemberProfileLink}}",
                        string.Concat(currentEnvironment, memberProfileBuilder.ToString()));
                    returnList.Add(new EmailToSend(member.Email, builder.ToString(), GetSubject(config, speakerName)));
                }

            }

            return returnList;
        }

        public string GetSubject(IConfigurationRoot config, string speakerName) =>
            new StringBuilder(config["NotifiedSpeakersSubject"])
                .Replace("{{SpeakerName}}", speakerName)
                .ToString();


        public class Member
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}