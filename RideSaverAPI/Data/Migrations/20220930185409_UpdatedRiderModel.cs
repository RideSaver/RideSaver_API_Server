using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideSaverAPI.Data.Migrations
{
    public partial class UpdatedRiderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiderCurrentDestination",
                table: "Riders");

            migrationBuilder.DropColumn(
                name: "RiderCurrentLocation",
                table: "Riders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RiderCurrentDestination",
                table: "Riders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiderCurrentLocation",
                table: "Riders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
