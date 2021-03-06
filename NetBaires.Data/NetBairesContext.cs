﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFSecondLevelCache.Core;
using EFSecondLevelCache.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetBaires.Data.Entities;

namespace NetBaires.Data
{

    public class NetBairesContext : DbContext
    {
        public NetBairesContext(DbContextOptions<NetBairesContext> options)
            : base(options)
        {
        }
        public NetBairesContext()
        {

        }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<FollowingMember> FollowingMembers { get; set; }

        public DbSet<Badge> Badges { get; set; }
        public DbSet<BadgeGroup> BadgeGroups { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<EventInformation> EventInformation { get; set; }
        public DbSet<BadgeMember> BadgeMembers { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<SponsorEvent> SponsorEvents { get; set; }
        public DbSet<GroupCode> GroupCodes { get; set; }
        public DbSet<GroupCodeMember> GroupCodeMembers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.UseLoggerFactory(_myLoggerFactory);
            base.OnConfiguring(optionsBuilder);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var changedEntityNames = this.GetChangedEntityNames();
            var domainEventEntities = ChangeTracker.Entries<Entity>()
                           .Select(po => po.Entity)
                           .Where(po => po.DomainEvents.Any())
                           .ToArray();
            var domainEvents = domainEventEntities.SelectMany(x => x.DomainEvents).Distinct();

            ChangeTracker.AutoDetectChangesEnabled = false; 
            var result = await base.SaveChangesAsync(cancellationToken); 
            ChangeTracker.AutoDetectChangesEnabled = true;

            this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);
            var queueServices = this.GetService<IQueueServices>();
            foreach (var domainEvent in domainEvents)
                    queueServices.AddMessage(domainEvent);

            return result;
        }

        public override int SaveChanges()
        {
            var changedEntityNames = this.GetChangedEntityNames();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges();
            ChangeTracker.AutoDetectChangesEnabled = true;

            this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);

            return result;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(t => t.GetForeignKeys())
            //    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            //foreach (var fk in cascadeFKs)
            //    fk.DeleteBehavior = DeleteBehavior.Restrict;
            modelBuilder
               .Entity<Template>()
               .Property(e => e.Type)
               .HasConversion(
                   v => v.ToString(),
                   v => (TemplateTypeEnum)Enum.Parse(typeof(TemplateTypeEnum), v));


            modelBuilder
                .Entity<Event>()
                .HasOne(x => x.EmailTemplateThanksSpeakers)
                .WithMany(x => x.EventsThanksSpeakers)
                .HasForeignKey(x => x.EmailTemplateThanksSpeakersId)
                .IsRequired(false);


            modelBuilder
                .Entity<Event>()
                .HasOne(x => x.EmailTemplateThanksSponsors)
                .WithMany(x => x.EventsThanksThanksSponsors)
                .HasForeignKey(x => x.EmailTemplateThanksSponsorsId)
                .IsRequired(false);

            modelBuilder
                .Entity<Event>()
                .HasOne(x => x.EmailTemplateThanksAttended)
                .WithMany(x => x.EventsThanksAttended)
                .HasForeignKey(x => x.EmailTemplateThanksAttendedId)
                .IsRequired(false);

            modelBuilder
                .Entity<Member>()
                .Property(e => e.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());
            modelBuilder.Entity<BadgeMember>().HasKey(sc => new { UserId = sc.MemberId, sc.BadgeId });
            modelBuilder.Entity<BadgeMember>()
                .HasOne<Badge>(sc => sc.Badge)
                .WithMany(s => s.Members)
                .HasForeignKey(sc => sc.BadgeId);


            //modelBuilder.Entity<FollowingMember>().HasKey(sc => new { sc.FollowingId, sc.MemberId });
            modelBuilder.Entity<FollowingMember>()
                .HasOne(x => x.Member)
                .WithMany(x => x.FollowingMembers)
                .HasForeignKey(sc => sc.FollowingId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);


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