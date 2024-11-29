using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class DefectUserChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Users_CausedByUserId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Users_ReportedByUserId",
                table: "Defects");

            migrationBuilder.RenameColumn(
                name: "ReportedByUserId",
                table: "Defects",
                newName: "CreatedByUserId");

            migrationBuilder.RenameColumn(
                name: "CausedByUserId",
                table: "Defects",
                newName: "DiscoveredByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Defects_ReportedByUserId",
                table: "Defects",
                newName: "IX_Defects_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Defects_CausedByUserId",
                table: "Defects",
                newName: "IX_Defects_DiscoveredByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Users_CreatedByUserId",
                table: "Defects",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Users_DiscoveredByUserId",
                table: "Defects",
                column: "DiscoveredByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Users_CreatedByUserId",
                table: "Defects");

            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Users_DiscoveredByUserId",
                table: "Defects");

            migrationBuilder.RenameColumn(
                name: "DiscoveredByUserId",
                table: "Defects",
                newName: "CausedByUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Defects",
                newName: "ReportedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Defects_DiscoveredByUserId",
                table: "Defects",
                newName: "IX_Defects_CausedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Defects_CreatedByUserId",
                table: "Defects",
                newName: "IX_Defects_ReportedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Users_CausedByUserId",
                table: "Defects",
                column: "CausedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Users_ReportedByUserId",
                table: "Defects",
                column: "ReportedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
