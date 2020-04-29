using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_Template_Enum_As_String : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Templates",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Templates",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
