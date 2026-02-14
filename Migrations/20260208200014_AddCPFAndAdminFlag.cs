using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozinhaApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCPFAndAdminFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CPFFuncionario",
                table: "Reservas",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Funcionarios",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Funcionarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_DataRefeicao_CPFFuncionario",
                table: "Reservas",
                columns: new[] { "DataRefeicao", "CPFFuncionario" });

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_CPF",
                table: "Funcionarios",
                column: "CPF",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservas_DataRefeicao_CPFFuncionario",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_CPF",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "CPFFuncionario",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Funcionarios");
        }
    }
}
