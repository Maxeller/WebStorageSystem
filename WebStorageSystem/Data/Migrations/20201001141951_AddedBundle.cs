using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class AddedBundle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartOfBundleId",
                table: "Units",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Bundles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    SerialNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bundles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_PartOfBundleId",
                table: "Units",
                column: "PartOfBundleId");

            migrationBuilder.CreateIndex(
                name: "IX_Bundles_SerialNumber",
                table: "Bundles",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Bundles_PartOfBundleId",
                table: "Units",
                column: "PartOfBundleId",
                principalTable: "Bundles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Bundles_PartOfBundleId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "Bundles");

            migrationBuilder.DropIndex(
                name: "IX_Units_PartOfBundleId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "PartOfBundleId",
                table: "Units");
        }
    }
}
