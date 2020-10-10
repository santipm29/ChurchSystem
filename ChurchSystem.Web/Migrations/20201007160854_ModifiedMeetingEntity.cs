using Microsoft.EntityFrameworkCore.Migrations;

namespace ChurchSystem.Web.Migrations
{
    public partial class ModifiedMeetingEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Districts_Name_FieldId",
                table: "Districts");

            migrationBuilder.AlterColumn<int>(
                name: "ChurchId",
                table: "Meetings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FieldId",
                table: "Districts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name_FieldId",
                table: "Districts",
                columns: new[] { "Name", "FieldId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings",
                column: "ChurchId",
                principalTable: "Churches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Districts_Name_FieldId",
                table: "Districts");

            migrationBuilder.AlterColumn<int>(
                name: "ChurchId",
                table: "Meetings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FieldId",
                table: "Districts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name_FieldId",
                table: "Districts",
                columns: new[] { "Name", "FieldId" },
                unique: true,
                filter: "[FieldId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings",
                column: "ChurchId",
                principalTable: "Churches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
