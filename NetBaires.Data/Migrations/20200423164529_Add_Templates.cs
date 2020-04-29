using Microsoft.EntityFrameworkCore.Migrations;

namespace NetBaires.Data.Migrations
{
    public partial class Add_Templates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Events",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                nullable: false,
                defaultValue:"",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailTemplateThanksAttendedId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailTemplateThanksSpeakersId",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailTemplateThanksSponsorsId",
                table: "Events",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TemplateContent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_EmailTemplateThanksAttendedId",
                table: "Events",
                column: "EmailTemplateThanksAttendedId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EmailTemplateThanksSpeakersId",
                table: "Events",
                column: "EmailTemplateThanksSpeakersId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EmailTemplateThanksSponsorsId",
                table: "Events",
                column: "EmailTemplateThanksSponsorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Templates_EmailTemplateThanksAttendedId",
                table: "Events",
                column: "EmailTemplateThanksAttendedId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Templates_EmailTemplateThanksSpeakersId",
                table: "Events",
                column: "EmailTemplateThanksSpeakersId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Templates_EmailTemplateThanksSponsorsId",
                table: "Events",
                column: "EmailTemplateThanksSponsorsId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Templates_EmailTemplateThanksAttendedId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Templates_EmailTemplateThanksSpeakersId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Templates_EmailTemplateThanksSponsorsId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Events_EmailTemplateThanksAttendedId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EmailTemplateThanksSpeakersId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EmailTemplateThanksSponsorsId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EmailTemplateThanksAttendedId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EmailTemplateThanksSpeakersId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EmailTemplateThanksSponsorsId",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
