using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class ChangeInAltKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Units_InventoryNumber",
                table: "Units");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Defects_DefectNumber",
                table: "Defects");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Bundles_InventoryNumber",
                table: "Bundles");

            migrationBuilder.CreateIndex(
                name: "IX_Units_InventoryNumber",
                table: "Units",
                column: "InventoryNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Defects_DefectNumber",
                table: "Defects",
                column: "DefectNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bundles_InventoryNumber",
                table: "Bundles",
                column: "InventoryNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_InventoryNumber",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Defects_DefectNumber",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_Bundles_InventoryNumber",
                table: "Bundles");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Units_InventoryNumber",
                table: "Units",
                column: "InventoryNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Defects_DefectNumber",
                table: "Defects",
                column: "DefectNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Bundles_InventoryNumber",
                table: "Bundles",
                column: "InventoryNumber");
        }
    }
}
