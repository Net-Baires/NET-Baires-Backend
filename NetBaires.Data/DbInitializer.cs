using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                    Role = UserRole.Admin
                });
            }

            context.Database.Migrate();
            context.SaveChanges();
        }
    }
}