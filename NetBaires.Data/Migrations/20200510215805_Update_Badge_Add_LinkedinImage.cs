using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_Badge_Add_LinkedinImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkedinImageName",
                table: "Badges",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedinImageUrl",
                table: "Badges",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkedinImageName",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "LinkedinImageUrl",
                table: "Badges");
        }
    }
}
