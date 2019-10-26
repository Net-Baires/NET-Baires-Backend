using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_MemberEvent_Remove_Rol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "EventMembers");

            migrationBuilder.AddColumn<bool>(
                name: "Organized",
                table: "EventMembers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Speaker",
                table: "EventMembers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Organized",
                table: "EventMembers");

            migrationBuilder.DropColumn(
                name: "Speaker",
                table: "EventMembers");

            migrationBuilder.AddColumn<int>(
                name: "Rol",
                table: "EventMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
