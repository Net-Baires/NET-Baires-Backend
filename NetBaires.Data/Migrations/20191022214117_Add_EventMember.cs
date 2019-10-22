using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Add_EventMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventMembers_Members_UserId",
                table: "EventMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventMembers",
                table: "EventMembers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EventMembers");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "EventMembers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Attended",
                table: "EventMembers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "EventMembers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventMembers",
                table: "EventMembers",
                columns: new[] { "MemberId", "EventId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventMembers_Members_MemberId",
                table: "EventMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventMembers_Members_MemberId",
                table: "EventMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventMembers",
                table: "EventMembers");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "EventMembers");

            migrationBuilder.DropColumn(
                name: "Attended",
                table: "EventMembers");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "EventMembers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "EventMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventMembers",
                table: "EventMembers",
                columns: new[] { "UserId", "EventId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventMembers_Members_UserId",
                table: "EventMembers",
                column: "UserId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
