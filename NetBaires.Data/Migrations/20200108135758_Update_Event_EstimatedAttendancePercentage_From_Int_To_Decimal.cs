using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_Event_EstimatedAttendancePercentage_From_Int_To_Decimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedAttendancePercentage",
                table: "Events",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EstimatedAttendancePercentage",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
