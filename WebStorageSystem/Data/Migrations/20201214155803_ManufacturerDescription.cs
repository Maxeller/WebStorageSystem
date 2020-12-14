using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class ManufacturerDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Manufacturers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Manufacturers");
        }
    }
}
