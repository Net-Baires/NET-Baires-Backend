using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_BadgeMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BadgeMembers_Members_UserId",
                table: "BadgeMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BadgeMembers",
                table: "BadgeMembers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BadgeMembers");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "BadgeMembers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BadgeMembers",
                table: "BadgeMembers",
                columns: new[] { "MemberId", "BadgeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BadgeMembers_Members_MemberId",
                table: "BadgeMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BadgeMembers_Members_MemberId",
                table: "BadgeMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BadgeMembers",
                table: "BadgeMembers");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "BadgeMembers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BadgeMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BadgeMembers",
                table: "BadgeMembers",
                columns: new[] { "UserId", "BadgeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BadgeMembers_Members_UserId",
                table: "BadgeMembers",
                column: "UserId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
