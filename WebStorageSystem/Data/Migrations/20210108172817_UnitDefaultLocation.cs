using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class UnitDefaultLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultLocationId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Units_DefaultLocationId",
                table: "Units",
                column: "DefaultLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Locations_DefaultLocationId",
                table: "Units",
                column: "DefaultLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Locations_DefaultLocationId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_DefaultLocationId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "DefaultLocationId",
                table: "Units");
        }
    }
}
