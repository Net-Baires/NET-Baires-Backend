using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

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
        public DbSet<BadgeGroup> BadgeGroups { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<BadgeMember> BadgeMembers { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<SponsorEvent> SponsorEvents { get; set; }
        public DbSet<GroupCode> GroupCodes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.UseLoggerFactory(_myLoggerFactory);
            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder
                .Entity<Member>()
                .Property(e => e.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());
            modelBuilder.Entity<BadgeMember>().HasKey(sc => new { UserId = sc.MemberId, sc.BadgeId });
            modelBuilder.Entity<BadgeMember>()
                .HasOne<Badge>(sc => sc.Badge)
                .WithMany(s => s.Members)
                .HasForeignKey(sc => sc.BadgeId);


            modelBuilder.Entity<BadgeMember>()
                .HasOne<Member>(sc => sc.Member)
                .WithMany(s => s.Badges)
                .HasForeignKey(sc => sc.MemberId);

            modelBuilder.Entity<Attendance>().HasKey(sc => new { sc.MemberId, sc.EventId });

            modelBuilder.Entity<Attendance>()
                .HasOne<Event>(sc => sc.Event)
                .WithMany(s => s.Attendees)
                .HasForeignKey(sc => sc.EventId);


            modelBuilder.Entity<Attendance>()
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

            modelBuilder
                .Entity<Event>()
                .Property(e => e.Platform)
                .HasConversion(
                    v => v.ToString(),
                    v => (EventPlatform)Enum.Parse(typeof(EventPlatform), v));


        }

    }
}