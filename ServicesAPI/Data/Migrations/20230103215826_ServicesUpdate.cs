using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicesAPI.Data.Migrations
{
    public partial class ServicesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_services_providers_ProviderId",
                table: "services");

            migrationBuilder.DropIndex(
                name: "IX_services_ProviderId",
                table: "services");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "services");

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderModelId",
                table: "services",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_services_ProviderModelId",
                table: "services",
                column: "ProviderModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_services_providers_ProviderModelId",
                table: "services",
                column: "ProviderModelId",
                principalTable: "providers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_services_providers_ProviderModelId",
                table: "services");

            migrationBuilder.DropIndex(
                name: "IX_services_ProviderModelId",
                table: "services");

            migrationBuilder.DropColumn(
                name: "ProviderModelId",
                table: "services");

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "services",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_services_ProviderId",
                table: "services",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_services_providers_ProviderId",
                table: "services",
                column: "ProviderId",
                principalTable: "providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
