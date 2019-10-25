using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_Member_Add_PictureName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureName",
                table: "Members",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureName",
                table: "Members");
        }
    }
}
