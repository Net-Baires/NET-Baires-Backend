using NetBaires.Data;
using System.Linq;
using NetBaires.Data.Entities;

namespace NetBaires.Api.Tests.Integration
{
    public static class UtilitiesDb
    {
        public static void ReinitializeDbForTests(NetBairesContext db)
        {
            db.Members.RemoveRange(db.Members);
        }
        internal static void InitializeDbForTests(NetBairesContext db)
        {
            var members = db.Members.ToList();
            db.Members.Add(new Member
            {
                Email = "admin@admin.com",
                FirstName = "Admin",
                LastName = "Admin",
                Role = UserRole.Admin
            });
            db.Members.Add(new Member
            {
                Email = "Member@Member.com",
                FirstName = "Member",
                LastName = "Member",
                Role = UserRole.Member
            });
            db.Members.Add(new Member
            {
                Email = "Organizer@Organizer.com",
                FirstName = "Organizer",
                LastName = "Organizer",
                Role = UserRole.Organizer
            });
            db.SaveChanges();
        }
    }
}