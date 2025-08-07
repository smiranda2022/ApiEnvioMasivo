using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiEnvioMasivo.Migrations
{
    public partial class AgregarClicsYRebotesEnCorreoEnviado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HizoClic",
                table: "CorreosEnviados",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Rebotado",
                table: "CorreosEnviados",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HizoClic",
                table: "CorreosEnviados");

            migrationBuilder.DropColumn(
                name: "Rebotado",
                table: "CorreosEnviados");
        }
    }
}
