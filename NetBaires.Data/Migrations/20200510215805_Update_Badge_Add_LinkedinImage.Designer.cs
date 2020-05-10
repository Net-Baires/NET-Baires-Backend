﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetBaires.Data;

namespace NetBaires.Data.Migrations
{
    [DbContext(typeof(NetBairesContext))]
    [Migration("20200510215805_Update_Badge_Add_LinkedinImage")]
    partial class Update_Badge_Add_LinkedinImage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetBaires.Data.Entities.Attendance", b =>
                {
                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("AttendanceRegisterType")
                        .HasColumnType("int");

                    b.Property<bool>("Attended")
                        .HasColumnType("bit");

                    b.Property<DateTime>("AttendedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("DidNotAttend")
                        .HasColumnType("bit");

                    b.Property<bool>("DoNotKnow")
                        .HasColumnType("bit");

                    b.Property<bool>("NotifiedAbsence")
                        .HasColumnType("bit");

                    b.Property<bool>("Organizer")
                        .HasColumnType("bit");

                    b.Property<bool>("Speaker")
                        .HasColumnType("bit");

                    b.HasKey("MemberId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.Badge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BadgeGroupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkedinImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkedinImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SimpleImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SimpleImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BadgeGroupId");

                    b.ToTable("Badges");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.BadgeGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BadgeGroups");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.BadgeMember", b =>
                {
                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int>("BadgeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AssignmentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("MemberId", "BadgeId");

                    b.HasIndex("BadgeId");

                    b.ToTable("BadgeMembers");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Done")
                        .HasColumnType("bit");

                    b.Property<int?>("EmailTemplateThanksAttendedId")
                        .HasColumnType("int");

                    b.Property<int?>("EmailTemplateThanksSpeakersId")
                        .HasColumnType("int");

                    b.Property<int?>("EmailTemplateThanksSponsorsId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndLiveTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("EstimatedAttendancePercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("EventId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("GeneralAttended")
                        .HasColumnType("bit");

                    b.Property<string>("GeneralAttendedCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Live")
                        .HasColumnType("bit");

                    b.Property<bool>("Online")
                        .HasColumnType("bit");

                    b.Property<string>("OnlineLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Platform")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartLiveTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmailTemplateThanksAttendedId");

                    b.HasIndex("EmailTemplateThanksSpeakersId");

                    b.HasIndex("EmailTemplateThanksSponsorsId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.EventInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Visible")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventInformation");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.FollowingMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FollowingDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FollowingId")
                        .HasColumnType("int");

                    b.Property<int?>("FollowingId1")
                        .HasColumnType("int");

                    b.Property<int?>("MemberId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FollowingId");

                    b.HasIndex("FollowingId1");

                    b.ToTable("FollowingMembers");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.GroupCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<bool>("Open")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("GroupCodes");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.GroupCodeBadge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BadgeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GroupCodeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BadgeId");

                    b.HasIndex("GroupCodeId");

                    b.ToTable("GroupCodeBadge");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.GroupCodeMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupCodeId")
                        .HasColumnType("int");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<bool>("Winner")
                        .HasColumnType("bit");

                    b.Property<int>("WinnerPosition")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupCodeId");

                    b.HasIndex("MemberId");

                    b.ToTable("GroupCodeMembers");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Biography")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Blocked")
                        .HasColumnType("bit");

                    b.Property<bool>("Colaborator")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventbriteId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FirstLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Github")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Instagram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Linkedin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("MeetupId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Organized")
                        .HasColumnType("bit");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PictureName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Twitter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkPosition")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.PushNotificationInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("PushNotificationId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("PushNotificationInformation");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.Sponsor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SiteUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sponsors");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.SponsorEvent", b =>
                {
                    b.Property<int>("SponsorId")
                        .HasColumnType("int");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<string>("Detail")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SponsorId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("SponsorEvents");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.Template", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.Attendance", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.Event", "Event")
                        .WithMany("Attendees")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NetBaires.Data.Entities.Member", "Member")
                        .WithMany("Events")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetBaires.Data.Entities.Badge", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.BadgeGroup", null)
                        .WithMany("Badges")
                        .HasForeignKey("BadgeGroupId");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.BadgeMember", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.Badge", "Badge")
                        .WithMany("Members")
                        .HasForeignKey("BadgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NetBaires.Data.Entities.Member", "Member")
                        .WithMany("Badges")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetBaires.Data.Entities.Event", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.Template", "EmailTemplateThanksAttended")
                        .WithMany("EventsThanksAttended")
                        .HasForeignKey("EmailTemplateThanksAttendedId");

                    b.HasOne("NetBaires.Data.Entities.Template", "EmailTemplateThanksSpeakers")
                        .WithMany("EventsThanksSpeakers")
                        .HasForeignKey("EmailTemplateThanksSpeakersId");

                    b.HasOne("NetBaires.Data.Entities.Template", "EmailTemplateThanksSponsors")
                        .WithMany("EventsThanksThanksSponsors")
                        .HasForeignKey("EmailTemplateThanksSponsorsId");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.EventInformation", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.Event", "Event")
                        .WithMany("Information")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetBaires.Data.Entities.FollowingMember", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.Member", "Member")
                        .WithMany("FollowingMembers")
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("NetBaires.Data.Entities.Member", "Following")
                        .WithMany()
                        .HasForeignKey("FollowingId1");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.GroupCode", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.Event", "Event")
                        .WithMany("GroupCodes")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetBaires.Data.Entities.GroupCodeBadge", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.Badge", "Badge")
                        .WithMany()
                        .HasForeignKey("BadgeId");

                    b.HasOne("NetBaires.Data.Entities.GroupCode", "GroupCode")
                        .WithMany("GroupCodeBadges")
                        .HasForeignKey("GroupCodeId");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.GroupCodeMember", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.GroupCode", "GroupCode")
                        .WithMany("Members")
                        .HasForeignKey("GroupCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NetBaires.Data.Entities.Member", "Member")
                        .WithMany("GroupCodes")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetBaires.Data.Entities.Material", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.Event", "Event")
                        .WithMany("Materials")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetBaires.Data.Entities.PushNotificationInformation", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.Member", null)
                        .WithMany("PushNotifications")
                        .HasForeignKey("MemberId");
                });

            modelBuilder.Entity("NetBaires.Data.Entities.SponsorEvent", b =>
                {
                    b.HasOne("NetBaires.Data.Entities.Event", "Event")
                        .WithMany("Sponsors")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NetBaires.Data.Entities.Sponsor", "Sponsor")
                        .WithMany("Events")
                        .HasForeignKey("SponsorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
