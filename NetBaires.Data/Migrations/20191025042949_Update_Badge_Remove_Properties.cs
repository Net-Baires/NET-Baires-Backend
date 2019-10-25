using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_Badge_Remove_Properties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BadgeId",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "BadgeImageUrl",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "BadgeUrl",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "IssuerUrl",
                table: "Badges");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BadgeId",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BadgeImageUrl",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BadgeUrl",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IssuerUrl",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
