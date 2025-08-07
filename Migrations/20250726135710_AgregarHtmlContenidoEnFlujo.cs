using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiEnvioMasivo.Migrations
{
    public partial class AgregarHtmlContenidoEnFlujo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlujoHistorial",
                table: "FlujoHistorial");

            migrationBuilder.DropIndex(
                name: "IX_FlujoHistorial_DestinatarioId",
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

  

            migrationBuilder.AddColumn<string>(
                name: "HtmlContenido",
                table: "Flujos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlujoHistoriales",
                table: "FlujoHistoriales",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FlujoHistoriales",
                table: "FlujoHistoriales");

            migrationBuilder.DropColumn(
                name: "FechaProgramada",
                table: "Flujos");

            migrationBuilder.DropColumn(
                name: "HtmlContenido",
                table: "Flujos");

            migrationBuilder.RenameTable(
                name: "FlujoHistoriales",
                newName: "FlujoHistorial");

            migrationBuilder.AddColumn<int>(
                name: "FlujoPasoId",
                table: "FlujoHistorial",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlujoHistorial",
                table: "FlujoHistorial",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FlujoHistorial_DestinatarioId",
                table: "FlujoHistorial",
                column: "DestinatarioId");

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
    }
}
