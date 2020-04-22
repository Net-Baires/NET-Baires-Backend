using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Add_Materials : Migration
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
                name: "FK_GroupCodeMembers_GroupCodes_GroupCodeId",
                table: "GroupCodeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodeMembers_Members_MemberId",
                table: "GroupCodeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodes_Events_EventId",
                table: "GroupCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorEvents_Events_EventId",
                table: "SponsorEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorEvents_Sponsors_SponsorId",
                table: "SponsorEvents");

            migrationBuilder.AlterColumn<int>(
                name: "FollowingId",
                table: "FollowingMembers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_EventId",
                table: "Materials",
                column: "EventId");

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
                name: "FK_GroupCodeMembers_GroupCodes_GroupCodeId",
                table: "GroupCodeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodeMembers_Members_MemberId",
                table: "GroupCodeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodes_Events_EventId",
                table: "GroupCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorEvents_Events_EventId",
                table: "SponsorEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_SponsorEvents_Sponsors_SponsorId",
                table: "SponsorEvents");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.AlterColumn<int>(
                name: "FollowingId",
                table: "FollowingMembers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

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
    }
}
