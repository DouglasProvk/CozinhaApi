using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozinhaApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEhPrincipalToPrato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EhPrincipal",
                table: "Pratos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EhPrincipal",
                table: "Pratos");
        }
    }
}
