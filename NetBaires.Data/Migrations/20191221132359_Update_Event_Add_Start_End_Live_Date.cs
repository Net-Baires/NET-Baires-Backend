using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_Event_Add_Start_End_Live_Date : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndLiveTime",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartLiveTime",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BadgeGroupId",
                table: "Badges",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Badges",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SimpleImageName",
                table: "Badges",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SimpleImageUrl",
                table: "Badges",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BadgeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadgeGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Badges_BadgeGroupId",
                table: "Badges",
                column: "BadgeGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_BadgeGroups_BadgeGroupId",
                table: "Badges",
                column: "BadgeGroupId",
                principalTable: "BadgeGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_BadgeGroups_BadgeGroupId",
                table: "Badges");

            migrationBuilder.DropTable(
                name: "BadgeGroups");

            migrationBuilder.DropIndex(
                name: "IX_Badges_BadgeGroupId",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "EndLiveTime",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StartLiveTime",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "BadgeGroupId",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "SimpleImageName",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "SimpleImageUrl",
                table: "Badges");
        }
    }
}
