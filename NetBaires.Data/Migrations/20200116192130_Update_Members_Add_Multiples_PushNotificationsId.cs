using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_Members_Add_Multiples_PushNotificationsId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PushNotificationId",
                table: "Members");

            migrationBuilder.CreateTable(
                name: "PushNotificationInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PushNotificationId = table.Column<string>(nullable: true),
                    MemberId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushNotificationInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PushNotificationInformation_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PushNotificationInformation_MemberId",
                table: "PushNotificationInformation",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PushNotificationInformation");

            migrationBuilder.AddColumn<string>(
                name: "PushNotificationId",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
