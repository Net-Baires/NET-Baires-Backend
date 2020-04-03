using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Add_Followed_Members : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowedMembers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(nullable: false),
                    FollowedId = table.Column<int>(nullable: true),
                    FollowingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowedMembers_Members_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowedMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowedMembers_FollowedId",
                table: "FollowedMembers",
                column: "FollowedId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedMembers_MemberId",
                table: "FollowedMembers",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowedMembers");
        }
    }
}
