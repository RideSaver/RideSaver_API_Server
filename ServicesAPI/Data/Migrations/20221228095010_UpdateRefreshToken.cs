using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicesAPI.Data.Migrations
{
    public partial class UpdateRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_areas_services_ServicesModelId",
                table: "service_areas");

            migrationBuilder.DropForeignKey(
                name: "FK_service_features_services_ServicesModelId",
                table: "service_features");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServicesModelId",
                table: "service_features",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServicesModelId",
                table: "service_areas",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_service_areas_services_ServicesModelId",
                table: "service_areas",
                column: "ServicesModelId",
                principalTable: "services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_service_features_services_ServicesModelId",
                table: "service_features",
                column: "ServicesModelId",
                principalTable: "services",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_areas_services_ServicesModelId",
                table: "service_areas");

            migrationBuilder.DropForeignKey(
                name: "FK_service_features_services_ServicesModelId",
                table: "service_features");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServicesModelId",
                table: "service_features",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServicesModelId",
                table: "service_areas",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_service_areas_services_ServicesModelId",
                table: "service_areas",
                column: "ServicesModelId",
                principalTable: "services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_service_features_services_ServicesModelId",
                table: "service_features",
                column: "ServicesModelId",
                principalTable: "services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
