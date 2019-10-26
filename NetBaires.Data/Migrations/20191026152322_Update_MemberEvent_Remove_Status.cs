using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_MemberEvent_Remove_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "EventMembers");

            migrationBuilder.AddColumn<bool>(
                name: "Attended",
                table: "EventMembers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DidNotAttend",
                table: "EventMembers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DoNotKnow",
                table: "EventMembers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotifiedAbsence",
                table: "EventMembers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attended",
                table: "EventMembers");

            migrationBuilder.DropColumn(
                name: "DidNotAttend",
                table: "EventMembers");

            migrationBuilder.DropColumn(
                name: "DoNotKnow",
                table: "EventMembers");

            migrationBuilder.DropColumn(
                name: "NotifiedAbsence",
                table: "EventMembers");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EventMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
