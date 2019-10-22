using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NetBaires.Data
{
    public class NetBairesContext : DbContext
    {
        public NetBairesContext(DbContextOptions<NetBairesContext> options)
            : base(options)
        {
        }


        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<BadgeMember> BadgeMembers { get; set; }
        public DbSet<EventMember> EventMembers { get; set; }
        public DbSet<SponsorEvent> SponsorEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Member>()
                .Property(e => e.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());
            modelBuilder.Entity<BadgeMember>().HasKey(sc => new { sc.UserId, sc.BadgeId });

            modelBuilder.Entity<BadgeMember>()
                .HasOne<Badge>(sc => sc.Badge)
                .WithMany(s => s.Users)
                .HasForeignKey(sc => sc.BadgeId);


            modelBuilder.Entity<BadgeMember>()
                .HasOne<Member>(sc => sc.User)
                .WithMany(s => s.Badges)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<EventMember>().HasKey(sc => new { sc.MemberId, sc.EventId });

            modelBuilder.Entity<EventMember>()
                .HasOne<Event>(sc => sc.Event)
                .WithMany(s => s.Attendees)
                .HasForeignKey(sc => sc.EventId);


            modelBuilder.Entity<EventMember>()
                .HasOne<Member>(sc => sc.Member)
                .WithMany(s => s.Events)
                .HasForeignKey(sc => sc.MemberId);

            modelBuilder.Entity<SponsorEvent>().HasKey(sc => new { sc.SponsorId, sc.EventId });

            modelBuilder.Entity<SponsorEvent>()
                .HasOne<Sponsor>(sc => sc.Sponsor)
                .WithMany(s => s.Events)
                .HasForeignKey(sc => sc.SponsorId);


            modelBuilder.Entity<SponsorEvent>()
                .HasOne<Event>(sc => sc.Event)
                .WithMany(s => s.Sponsors)
                .HasForeignKey(sc => sc.EventId);

        }

    }
}