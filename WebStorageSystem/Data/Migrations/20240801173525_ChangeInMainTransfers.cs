using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class ChangeInMainTransfers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DestinationLocationId",
                table: "MainTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MainTransfers_DestinationLocationId",
                table: "MainTransfers",
                column: "DestinationLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MainTransfers_Locations_DestinationLocationId",
                table: "MainTransfers",
                column: "DestinationLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainTransfers_Locations_DestinationLocationId",
                table: "MainTransfers");

            migrationBuilder.DropIndex(
                name: "IX_MainTransfers_DestinationLocationId",
                table: "MainTransfers");

            migrationBuilder.DropColumn(
                name: "DestinationLocationId",
                table: "MainTransfers");
        }
    }
}
