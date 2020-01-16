using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_BadgeMember_Add_AssignmentDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BadgeUrl",
                table: "BadgeMembers");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignmentDate",
                table: "BadgeMembers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentDate",
                table: "BadgeMembers");

            migrationBuilder.AddColumn<string>(
                name: "BadgeUrl",
                table: "BadgeMembers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
