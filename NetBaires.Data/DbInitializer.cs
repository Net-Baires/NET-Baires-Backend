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

            context.Database.Migrate();
            context.SaveChanges();
        }
    }
}