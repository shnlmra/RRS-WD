using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRS.Migrations
{
    /// <inheritdoc />
    public partial class AddBuffetTypeIdInReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuffetTypeId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BuffetTypeId",
                table: "Reservations",
                column: "BuffetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_BuffetTypes_BuffetTypeId",
                table: "Reservations",
                column: "BuffetTypeId",
                principalTable: "BuffetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_BuffetTypes_BuffetTypeId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_BuffetTypeId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BuffetTypeId",
                table: "Reservations");
        }
    }
}
