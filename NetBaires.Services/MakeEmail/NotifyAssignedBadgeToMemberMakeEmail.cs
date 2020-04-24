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
    public class NotifyAssignedBadgeToMemberMakeEmail : IMakeEmail<AssignedBadgeToMember>
    {
        private string _memberName;
        private Badge _badge;
        private Member _member;

        public List<EmailToSend> Make(AssignedBadgeToMember data, IConfigurationRoot config)
        {
            var emailToSend = new List<EmailToSend>();
            var currentEnvironment = config["CurrentEnvironment"];
            var connectionString = config["ConnectionString"];
            var badgeLinkBuilder = new StringBuilder(config["BadgeLink"]);

            using (var connection = new SqlConnection(connectionString))
            {
                var template = connection
                    .Query<Template>($"SELECT TemplateContent FROM Templates Where Type = 3")
                    .FirstOrDefault();
                var builder = new StringBuilder(template.TemplateContent);

                _member = connection.Query<Member>($"SELECT Email,FirstName,LastName FROM Members WHERE ID = {data.MemberId}").FirstOrDefault();
                if (_member == null || string.IsNullOrWhiteSpace(_member.Email))
                    throw new UserDoesNotHaveEmailException(data.MemberId);
                _memberName = string.Concat(_member.FirstName, " ", _member.LastName);
                builder.Replace("{{MemberName}}", _memberName);

                _badge = connection.Query<Badge>($"SELECT ImageUrl,Name,Description FROM Badges WHERE ID = {data.BadgeId}").FirstOrDefault();

                builder.Replace("{{BadgeName}}", _badge.Name);
                builder.Replace("{{BadgeImageUrl}}", _badge.ImageUrl);
                badgeLinkBuilder.Replace("{{MemberId}}", data.MemberId.ToString());
                badgeLinkBuilder.Replace("{{BadgeId}}", data.BadgeId.ToString());


                builder.Replace("{{BadgeLink}}", string.Concat(currentEnvironment, badgeLinkBuilder.ToString()));
                emailToSend.Add(new EmailToSend(_member.Email, builder.ToString(), GetSubject(config)));
            }

           
            return emailToSend;
        }



        private string GetSubject(IConfigurationRoot config)
        {
            return new StringBuilder(config["NotifyAssignedBadgeToAttendanceSubject"])
                .Replace("{{MemberName}}", _memberName)
                .Replace("{{BadgeName}}", _badge.Name)
                .ToString();
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