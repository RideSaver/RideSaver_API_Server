using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideSaverAPI.Data.Migrations
{
    public partial class AddRiderAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RiderEmail",
                table: "Riders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RiderPassword",
                table: "Riders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RiderUsername",
                table: "Riders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiderEmail",
                table: "Riders");

            migrationBuilder.DropColumn(
                name: "RiderPassword",
                table: "Riders");

            migrationBuilder.DropColumn(
                name: "RiderUsername",
                table: "Riders");
        }
    }
}
