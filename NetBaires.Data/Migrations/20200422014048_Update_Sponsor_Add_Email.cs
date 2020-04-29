using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_Sponsor_Add_Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Events_EventId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Members_MemberId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_BadgeMembers_Badges_BadgeId",
                table: "BadgeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_BadgeMembers_Members_MemberId",
                table: "BadgeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowingMembers_Members_MemberId",
                table: "FollowingMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodeMembers_GroupCodes_GroupCodeId",
                table: "GroupCodeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodeMembers_Members_MemberId",
                table: "GroupCodeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodes_Events_EventId",
                table: "GroupCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Events_EventId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorEvents_Events_EventId",
                table: "SponsorEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorEvents_Sponsors_SponsorId",
                table: "SponsorEvents");

            migrationBuilder.DropIndex(
                name: "IX_FollowingMembers_MemberId",
                table: "FollowingMembers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Sponsors",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "FollowingMembers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FollowingId",
                table: "FollowingMembers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FollowingId1",
                table: "FollowingMembers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FollowingMembers_FollowingId1",
                table: "FollowingMembers",
                column: "FollowingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Events_EventId",
                table: "Attendances",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Members_MemberId",
                table: "Attendances",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BadgeMembers_Badges_BadgeId",
                table: "BadgeMembers",
                column: "BadgeId",
                principalTable: "Badges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BadgeMembers_Members_MemberId",
                table: "BadgeMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowingMembers_Members_FollowingId1",
                table: "FollowingMembers",
                column: "FollowingId1",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCodeMembers_GroupCodes_GroupCodeId",
                table: "GroupCodeMembers",
                column: "GroupCodeId",
                principalTable: "GroupCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCodeMembers_Members_MemberId",
                table: "GroupCodeMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCodes_Events_EventId",
                table: "GroupCodes",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Events_EventId",
                table: "Materials",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SponsorEvents_Events_EventId",
                table: "SponsorEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SponsorEvents_Sponsors_SponsorId",
                table: "SponsorEvents",
                column: "SponsorId",
                principalTable: "Sponsors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Events_EventId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Members_MemberId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_BadgeMembers_Badges_BadgeId",
                table: "BadgeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_BadgeMembers_Members_MemberId",
                table: "BadgeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowingMembers_Members_FollowingId1",
                table: "FollowingMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodeMembers_GroupCodes_GroupCodeId",
                table: "GroupCodeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodeMembers_Members_MemberId",
                table: "GroupCodeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodes_Events_EventId",
                table: "GroupCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Events_EventId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorEvents_Events_EventId",
                table: "SponsorEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorEvents_Sponsors_SponsorId",
                table: "SponsorEvents");

            migrationBuilder.DropIndex(
                name: "IX_FollowingMembers_FollowingId1",
                table: "FollowingMembers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Sponsors");

            migrationBuilder.DropColumn(
                name: "FollowingId1",
                table: "FollowingMembers");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "FollowingMembers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FollowingId",
                table: "FollowingMembers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FollowingMembers_MemberId",
                table: "FollowingMembers",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Events_EventId",
                table: "Attendances",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Members_MemberId",
                table: "Attendances",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BadgeMembers_Badges_BadgeId",
                table: "BadgeMembers",
                column: "BadgeId",
                principalTable: "Badges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BadgeMembers_Members_MemberId",
                table: "BadgeMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowingMembers_Members_MemberId",
                table: "FollowingMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCodeMembers_GroupCodes_GroupCodeId",
                table: "GroupCodeMembers",
                column: "GroupCodeId",
                principalTable: "GroupCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCodeMembers_Members_MemberId",
                table: "GroupCodeMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCodes_Events_EventId",
                table: "GroupCodes",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Events_EventId",
                table: "Materials",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SponsorEvents_Events_EventId",
                table: "SponsorEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SponsorEvents_Sponsors_SponsorId",
                table: "SponsorEvents",
                column: "SponsorId",
                principalTable: "Sponsors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
