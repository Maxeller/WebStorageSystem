using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorageSystem.Data.Migrations
{
    public partial class LocationSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "ApplicationUserLocation",
                columns: table => new
                {
                    SubscribedLocationsId = table.Column<int>(type: "int", nullable: false),
                    UsersSubscribedId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLocation", x => new { x.SubscribedLocationsId, x.UsersSubscribedId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserLocation_Locations_SubscribedLocationsId",
                        column: x => x.SubscribedLocationsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserLocation_Users_UsersSubscribedId",
                        column: x => x.UsersSubscribedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLocation_UsersSubscribedId",
                table: "ApplicationUserLocation",
                column: "UsersSubscribedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserLocation");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
