using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Add_GroupCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupCodeId",
                table: "Members",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    Open = table.Column<bool>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupCodes_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_GroupCodeId",
                table: "Members",
                column: "GroupCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupCodes_EventId",
                table: "GroupCodes",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_GroupCodes_GroupCodeId",
                table: "Members",
                column: "GroupCodeId",
                principalTable: "GroupCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_GroupCodes_GroupCodeId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "GroupCodes");

            migrationBuilder.DropIndex(
                name: "IX_Members_GroupCodeId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "GroupCodeId",
                table: "Members");
        }
    }
}
