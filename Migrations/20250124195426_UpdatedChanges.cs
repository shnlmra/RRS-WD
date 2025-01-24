using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRS.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuffetTypes_Reservations_ReservationId",
                table: "BuffetTypes");

            migrationBuilder.DropIndex(
                name: "IX_BuffetTypes_ReservationId",
                table: "BuffetTypes");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "BuffetTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "BuffetTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuffetTypes_ReservationId",
                table: "BuffetTypes",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuffetTypes_Reservations_ReservationId",
                table: "BuffetTypes",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
