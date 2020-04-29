using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetBaires.Data.Entities;

namespace NetBaires.Data
{
    public static class DbInitializer
    {
        public static void Initialize(NetBairesContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Members.Any(x => x.Email == "german.kuber@outlook.com"))
            {
                context.Members.AddAsync(new Member
                {
                    Email = "german.kuber@outlook.com",
                    Role = UserRole.Admin,
                    MeetupId = 182823912,
                    FirstName = "Germán",
                    LastName = "Küber",
                    Github = "germankuber",
                    Instagram = "germankuber",
                    Twitter = "germankuber",
                    Username = "GermanKuber",
                    Organized = true
                });
            }

            if (!context.Templates.Any(x => x.Type == TemplateTypeEnum.EmailTemplateThanksAttended))
                context.Templates.AddAsync(new Template
                {
                    Name = "Agradecimiento a asistentes del evento Mensual NET-Baires",
                    Description = "Template para el envio mensual",
                    TemplateContent = "",
                    Type = TemplateTypeEnum.EmailTemplateThanksAttended
                });
            if (!context.Templates.Any(x => x.Type == TemplateTypeEnum.EmailTemplateThanksSponsors))
                context.Templates.AddAsync(new Template
                {
                    Name = "Agradecimiento a sponsors de evento Mensual NET-Baires",
                    Description = "Template para el envio mensual",
                    TemplateContent = "",
                    Type = TemplateTypeEnum.EmailTemplateThanksSponsors
                });
            if (!context.Templates.Any(x => x.Type == TemplateTypeEnum.EmailTemplateThanksSpeakers))
                context.Templates.AddAsync(new Template
                {
                    Name = "Agradecimiento a Speakers del evento Mensual NET-Baires",
                    Description = "Template para el envio mensual",
                    TemplateContent = "",
                    Type = TemplateTypeEnum.EmailTemplateThanksSpeakers
                });

            if (!context.Templates.Any(x => x.Type == TemplateTypeEnum.EmailTemplateAssignedBadgeToMember))
                context.Templates.AddAsync(new Template
                {
                    Name = "Notificación de nuevo Badge",
                    Description = "Template para Badge",
                    TemplateContent = "",
                    Type = TemplateTypeEnum.EmailTemplateAssignedBadgeToMember
                });

            context.Database.Migrate();
            context.SaveChanges();
        }
    }
}