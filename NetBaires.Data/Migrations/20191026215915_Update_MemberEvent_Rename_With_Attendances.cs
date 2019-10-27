using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Update_MemberEvent_Rename_With_Attendances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventMembers");

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    AttendedTime = table.Column<DateTime>(nullable: false),
                    Organizer = table.Column<bool>(nullable: false),
                    Speaker = table.Column<bool>(nullable: false),
                    DidNotAttend = table.Column<bool>(nullable: false),
                    Attended = table.Column<bool>(nullable: false),
                    NotifiedAbsence = table.Column<bool>(nullable: false),
                    DoNotKnow = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => new { x.MemberId, x.EventId });
                    table.ForeignKey(
                        name: "FK_Attendances_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EventId",
                table: "Attendances",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.CreateTable(
                name: "EventMembers",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    Attended = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DidNotAttend = table.Column<bool>(type: "bit", nullable: false),
                    DoNotKnow = table.Column<bool>(type: "bit", nullable: false),
                    NotifiedAbsence = table.Column<bool>(type: "bit", nullable: false),
                    Organizer = table.Column<bool>(type: "bit", nullable: false),
                    Speaker = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventMembers", x => new { x.MemberId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventMembers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventMembers_EventId",
                table: "EventMembers",
                column: "EventId");
        }
    }
}
