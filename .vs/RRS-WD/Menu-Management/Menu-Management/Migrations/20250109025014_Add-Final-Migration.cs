using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menu_Management.Migrations
{
    /// <inheritdoc />
    public partial class AddFinalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Add_Menu_Modal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Add_Menu_Modal");
        }
    }
}
