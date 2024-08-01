using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class ChangesInBundle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainTransfers_Locations_DestinationLocationId",
                table: "MainTransfers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Bundles_SerialNumber",
                table: "Bundles");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "Bundles",
                newName: "InventoryNumber");

            migrationBuilder.AddColumn<int>(
                name: "BundleId",
                table: "SubTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Bundles_InventoryNumber",
                table: "Bundles",
                column: "InventoryNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SubTransfers_BundleId",
                table: "SubTransfers",
                column: "BundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_MainTransfers_Locations_DestinationLocationId",
                table: "MainTransfers",
                column: "DestinationLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTransfers_Bundles_BundleId",
                table: "SubTransfers",
                column: "BundleId",
                principalTable: "Bundles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainTransfers_Locations_DestinationLocationId",
                table: "MainTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTransfers_Bundles_BundleId",
                table: "SubTransfers");

            migrationBuilder.DropIndex(
                name: "IX_SubTransfers_BundleId",
                table: "SubTransfers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Bundles_InventoryNumber",
                table: "Bundles");

            migrationBuilder.DropColumn(
                name: "BundleId",
                table: "SubTransfers");

            migrationBuilder.RenameColumn(
                name: "InventoryNumber",
                table: "Bundles",
                newName: "SerialNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Bundles_SerialNumber",
                table: "Bundles",
                column: "SerialNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_MainTransfers_Locations_DestinationLocationId",
                table: "MainTransfers",
                column: "DestinationLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
