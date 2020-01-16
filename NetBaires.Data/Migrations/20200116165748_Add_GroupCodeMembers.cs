using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Add_GroupCodeMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodeMember_GroupCodes_GroupCodeId",
                table: "GroupCodeMember");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodeMember_Members_MemberId",
                table: "GroupCodeMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupCodeMember",
                table: "GroupCodeMember");

            migrationBuilder.RenameTable(
                name: "GroupCodeMember",
                newName: "GroupCodeMembers");

            migrationBuilder.RenameIndex(
                name: "IX_GroupCodeMember_MemberId",
                table: "GroupCodeMembers",
                newName: "IX_GroupCodeMembers_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupCodeMember_GroupCodeId",
                table: "GroupCodeMembers",
                newName: "IX_GroupCodeMembers_GroupCodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupCodeMembers",
                table: "GroupCodeMembers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCodeMembers_GroupCodes_GroupCodeId",
                table: "GroupCodeMembers",
                column: "GroupCodeId",
                principalTable: "GroupCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCodeMembers_Members_MemberId",
                table: "GroupCodeMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodeMembers_GroupCodes_GroupCodeId",
                table: "GroupCodeMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupCodeMembers_Members_MemberId",
                table: "GroupCodeMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupCodeMembers",
                table: "GroupCodeMembers");

            migrationBuilder.RenameTable(
                name: "GroupCodeMembers",
                newName: "GroupCodeMember");

            migrationBuilder.RenameIndex(
                name: "IX_GroupCodeMembers_MemberId",
                table: "GroupCodeMember",
                newName: "IX_GroupCodeMember_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupCodeMembers_GroupCodeId",
                table: "GroupCodeMember",
                newName: "IX_GroupCodeMember_GroupCodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupCodeMember",
                table: "GroupCodeMember",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCodeMember_GroupCodes_GroupCodeId",
                table: "GroupCodeMember",
                column: "GroupCodeId",
                principalTable: "GroupCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupCodeMember_Members_MemberId",
                table: "GroupCodeMember",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
