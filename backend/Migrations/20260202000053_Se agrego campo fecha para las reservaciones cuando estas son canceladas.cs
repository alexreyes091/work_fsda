using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.webapi.backoffice_viajes_altairis.Migrations
{
    /// <inheritdoc />
    public partial class Seagregocampofechaparalasreservacionescuandoestassoncanceladas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "Reservations",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Reservations");
        }
    }
}
