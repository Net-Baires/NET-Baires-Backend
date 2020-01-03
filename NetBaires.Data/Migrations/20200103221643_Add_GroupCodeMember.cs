using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Add_GroupCodeMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_GroupCodes_GroupCodeId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_GroupCodeId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "GroupCodeId",
                table: "Members");

            migrationBuilder.CreateTable(
                name: "GroupCodeMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupCodeId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupCodeMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupCodeMember_GroupCodes_GroupCodeId",
                        column: x => x.GroupCodeId,
                        principalTable: "GroupCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupCodeMember_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupCodeMember_GroupCodeId",
                table: "GroupCodeMember",
                column: "GroupCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupCodeMember_MemberId",
                table: "GroupCodeMember",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupCodeMember");

            migrationBuilder.AddColumn<int>(
                name: "GroupCodeId",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_GroupCodeId",
                table: "Members",
                column: "GroupCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_GroupCodes_GroupCodeId",
                table: "Members",
                column: "GroupCodeId",
                principalTable: "GroupCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
