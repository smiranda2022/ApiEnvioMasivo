using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiEnvioMasivo.Migrations
{
    public partial class AgregarTrackingCampos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaApertura",
                table: "CorreosEnviados",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PasoId",
                table: "CorreosEnviados",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SuscripcionRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorreosEnviados_PasoId",
                table: "CorreosEnviados",
                column: "PasoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CorreosEnviados_FlujoPasos_PasoId",
                table: "CorreosEnviados",
                column: "PasoId",
                principalTable: "FlujoPasos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorreosEnviados_FlujoPasos_PasoId",
                table: "CorreosEnviados");

            migrationBuilder.DropTable(
                name: "SuscripcionRequest");

            migrationBuilder.DropIndex(
                name: "IX_CorreosEnviados_PasoId",
                table: "CorreosEnviados");

            migrationBuilder.DropColumn(
                name: "FechaApertura",
                table: "CorreosEnviados");

            migrationBuilder.DropColumn(
                name: "PasoId",
                table: "CorreosEnviados");
        }
    }
}
