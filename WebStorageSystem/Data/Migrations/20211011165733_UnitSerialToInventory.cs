using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class UnitSerialToInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Units_SerialNumber",
                table: "Units");

            migrationBuilder.AlterColumn<string>(
                name: "ShelfNumber",
                table: "Units",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Units",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "InventoryNumber",
                table: "Units",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Units_InventoryNumber",
                table: "Units",
                column: "InventoryNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Units_InventoryNumber",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "InventoryNumber",
                table: "Units");

            migrationBuilder.AlterColumn<string>(
                name: "ShelfNumber",
                table: "Units",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Units",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Units_SerialNumber",
                table: "Units",
                column: "SerialNumber");
        }
    }
}
