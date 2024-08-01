using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class ChangeInSubTransfers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubTransferUnit");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "SubTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubTransfers_UnitId",
                table: "SubTransfers",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTransfers_Units_UnitId",
                table: "SubTransfers",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTransfers_Units_UnitId",
                table: "SubTransfers");

            migrationBuilder.DropIndex(
                name: "IX_SubTransfers_UnitId",
                table: "SubTransfers");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "SubTransfers");

            migrationBuilder.CreateTable(
                name: "SubTransferUnit",
                columns: table => new
                {
                    SubTransfersId = table.Column<int>(type: "int", nullable: false),
                    UnitsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTransferUnit", x => new { x.SubTransfersId, x.UnitsId });
                    table.ForeignKey(
                        name: "FK_SubTransferUnit_SubTransfers_SubTransfersId",
                        column: x => x.SubTransfersId,
                        principalTable: "SubTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubTransferUnit_Units_UnitsId",
                        column: x => x.UnitsId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubTransferUnit_UnitsId",
                table: "SubTransferUnit",
                column: "UnitsId");
        }
    }
}
