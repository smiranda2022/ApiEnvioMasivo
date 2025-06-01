using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiEnvioMasivo.Migrations
{
    public partial class CorreosEnviados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Espera",
                table: "FlujoPasos",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioFlujo",
                table: "Destinatarios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FlujoId",
                table: "Destinatarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CorreosEnviados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinatarioId = table.Column<int>(type: "int", nullable: false),
                    FlujoPasoId = table.Column<int>(type: "int", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Abierto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorreosEnviados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorreosEnviados_Destinatarios_DestinatarioId",
                        column: x => x.DestinatarioId,
                        principalTable: "Destinatarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorreosEnviados_FlujoPasos_FlujoPasoId",
                        column: x => x.FlujoPasoId,
                        principalTable: "FlujoPasos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinatarios_FlujoId",
                table: "Destinatarios",
                column: "FlujoId");

            migrationBuilder.CreateIndex(
                name: "IX_CorreosEnviados_DestinatarioId",
                table: "CorreosEnviados",
                column: "DestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CorreosEnviados_FlujoPasoId",
                table: "CorreosEnviados",
                column: "FlujoPasoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinatarios_Flujos_FlujoId",
                table: "Destinatarios",
                column: "FlujoId",
                principalTable: "Flujos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinatarios_Flujos_FlujoId",
                table: "Destinatarios");

            migrationBuilder.DropTable(
                name: "CorreosEnviados");

            migrationBuilder.DropIndex(
                name: "IX_Destinatarios_FlujoId",
                table: "Destinatarios");

            migrationBuilder.DropColumn(
                name: "Espera",
                table: "FlujoPasos");

            migrationBuilder.DropColumn(
                name: "FechaInicioFlujo",
                table: "Destinatarios");

            migrationBuilder.DropColumn(
                name: "FlujoId",
                table: "Destinatarios");
        }
    }
}
