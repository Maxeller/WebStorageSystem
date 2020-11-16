using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class EFCore5Upgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferUnits");

            migrationBuilder.CreateTable(
                name: "TransferUnit",
                columns: table => new
                {
                    PartOfTransfersId = table.Column<int>(type: "int", nullable: false),
                    TransferredUnitsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferUnit", x => new { x.PartOfTransfersId, x.TransferredUnitsId });
                    table.ForeignKey(
                        name: "FK_TransferUnit_Transfers_PartOfTransfersId",
                        column: x => x.PartOfTransfersId,
                        principalTable: "Transfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferUnit_Units_TransferredUnitsId",
                        column: x => x.TransferredUnitsId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferUnit_TransferredUnitsId",
                table: "TransferUnit",
                column: "TransferredUnitsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferUnit");

            migrationBuilder.CreateTable(
                name: "TransferUnits",
                columns: table => new
                {
                    TransferId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferUnits", x => new { x.TransferId, x.UnitId });
                    table.ForeignKey(
                        name: "FK_TransferUnits_Transfers_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferUnits_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferUnits_UnitId",
                table: "TransferUnits",
                column: "UnitId");
        }
    }
}
