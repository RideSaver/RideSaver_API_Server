using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideSaverAPI.Data.Migrations
{
    public partial class createdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Riders",
                columns: table => new
                {
                    RiderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiderPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiderCurrentLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiderCurrentDestination = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Riders", x => x.RiderID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Riders");
        }
    }
}
