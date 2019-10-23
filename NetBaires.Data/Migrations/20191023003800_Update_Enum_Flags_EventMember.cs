using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_Enum_Flags_EventMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attended",
                table: "EventMembers");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EventMembers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "EventMembers");

            migrationBuilder.AddColumn<bool>(
                name: "Attended",
                table: "EventMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
