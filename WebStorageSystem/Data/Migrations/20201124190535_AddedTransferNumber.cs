using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class AddedTransferNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransferNumber",
                table: "Transfers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_TransferNumber",
                table: "Transfers",
                column: "TransferNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transfers_TransferNumber",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "TransferNumber",
                table: "Transfers");
        }
    }
}
