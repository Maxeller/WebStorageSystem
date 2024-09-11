using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class ChangeInDefects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Images_ImageId",
                table: "Defects");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "Defects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Images_ImageId",
                table: "Defects",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Images_ImageId",
                table: "Defects");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "Defects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Images_ImageId",
                table: "Defects",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
