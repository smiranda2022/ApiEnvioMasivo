using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiEnvioMasivo.Migrations
{
    public partial class AgregarFlujoHistorialBien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlujoHistoriales_Destinatarios_DestinatarioId",
                table: "FlujoHistoriales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlujoHistoriales",
                table: "FlujoHistoriales");

            migrationBuilder.RenameTable(
                name: "FlujoHistoriales",
                newName: "FlujoHistorial");

            migrationBuilder.RenameIndex(
                name: "IX_FlujoHistoriales_DestinatarioId",
                table: "FlujoHistorial",
                newName: "IX_FlujoHistorial_DestinatarioId");

            migrationBuilder.AddColumn<int>(
                name: "FlujoPasoId",
                table: "FlujoHistorial",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlujoHistorial",
                table: "FlujoHistorial",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlujoHistorial_FlujoId",
                table: "FlujoHistorial",
                column: "FlujoId");

            migrationBuilder.CreateIndex(
                name: "IX_FlujoHistorial_FlujoPasoId",
                table: "FlujoHistorial",
                column: "FlujoPasoId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlujoHistorial_Destinatarios_DestinatarioId",
                table: "FlujoHistorial",
                column: "DestinatarioId",
                principalTable: "Destinatarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlujoHistorial_FlujoPasos_FlujoPasoId",
                table: "FlujoHistorial",
                column: "FlujoPasoId",
                principalTable: "FlujoPasos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlujoHistorial_Flujos_FlujoId",
                table: "FlujoHistorial",
                column: "FlujoId",
                principalTable: "Flujos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlujoHistorial_Destinatarios_DestinatarioId",
                table: "FlujoHistorial");

            migrationBuilder.DropForeignKey(
                name: "FK_FlujoHistorial_FlujoPasos_FlujoPasoId",
                table: "FlujoHistorial");

            migrationBuilder.DropForeignKey(
                name: "FK_FlujoHistorial_Flujos_FlujoId",
                table: "FlujoHistorial");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlujoHistorial",
                table: "FlujoHistorial");

            migrationBuilder.DropIndex(
                name: "IX_FlujoHistorial_FlujoId",
                table: "FlujoHistorial");

            migrationBuilder.DropIndex(
                name: "IX_FlujoHistorial_FlujoPasoId",
                table: "FlujoHistorial");

            migrationBuilder.DropColumn(
                name: "FlujoPasoId",
                table: "FlujoHistorial");

            migrationBuilder.RenameTable(
                name: "FlujoHistorial",
                newName: "FlujoHistoriales");

            migrationBuilder.RenameIndex(
                name: "IX_FlujoHistorial_DestinatarioId",
                table: "FlujoHistoriales",
                newName: "IX_FlujoHistoriales_DestinatarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlujoHistoriales",
                table: "FlujoHistoriales",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlujoHistoriales_Destinatarios_DestinatarioId",
                table: "FlujoHistoriales",
                column: "DestinatarioId",
                principalTable: "Destinatarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
