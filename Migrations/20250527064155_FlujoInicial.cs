using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiEnvioMasivo.Migrations
{
    public partial class FlujoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlujoHistoriales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinatarioId = table.Column<int>(type: "int", nullable: false),
                    FlujoId = table.Column<int>(type: "int", nullable: false),
                    PasoId = table.Column<int>(type: "int", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Abierto = table.Column<bool>(type: "bit", nullable: false),
                    HizoClic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlujoHistoriales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlujoHistoriales_Destinatarios_DestinatarioId",
                        column: x => x.DestinatarioId,
                        principalTable: "Destinatarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flujos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flujos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlujoPasos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlujoId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Asunto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HtmlContenido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiasDespuesDelInicio = table.Column<int>(type: "int", nullable: false),
                    CondicionTipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CondicionValor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlujoPasos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlujoPasos_Flujos_FlujoId",
                        column: x => x.FlujoId,
                        principalTable: "Flujos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlujoHistoriales_DestinatarioId",
                table: "FlujoHistoriales",
                column: "DestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_FlujoPasos_FlujoId",
                table: "FlujoPasos",
                column: "FlujoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlujoHistoriales");

            migrationBuilder.DropTable(
                name: "FlujoPasos");

            migrationBuilder.DropTable(
                name: "Flujos");
        }
    }
}
