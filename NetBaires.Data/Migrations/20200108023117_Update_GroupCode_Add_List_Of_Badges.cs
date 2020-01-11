using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_GroupCode_Add_List_Of_Badges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupCodeBadge",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupCodeId = table.Column<int>(nullable: true),
                    BadgeId = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupCodeBadge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupCodeBadge_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupCodeBadge_GroupCodes_GroupCodeId",
                        column: x => x.GroupCodeId,
                        principalTable: "GroupCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupCodeBadge_BadgeId",
                table: "GroupCodeBadge",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupCodeBadge_GroupCodeId",
                table: "GroupCodeBadge",
                column: "GroupCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupCodeBadge");
        }
    }
}
