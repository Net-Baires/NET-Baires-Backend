using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_MemberEvent_Organizer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Organized",
                table: "EventMembers");

            migrationBuilder.AddColumn<bool>(
                name: "Organizer",
                table: "EventMembers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Organizer",
                table: "EventMembers");

            migrationBuilder.AddColumn<bool>(
                name: "Organized",
                table: "EventMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
