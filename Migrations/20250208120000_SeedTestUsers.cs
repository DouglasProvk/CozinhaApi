using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozinhaApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Admin user
            migrationBuilder.InsertData(
                table: "Funcionarios",
                columns: new[] { "Nome", "Email", "CPF", "Telefone", "Cargo", "Departamento", "Ativo", "IsAdmin", "DataAdmissao" },
                values: new object[,]
                {
                    {
                        "Admin Sistema",
                        "admin@cozinha.com",
                        "11111111111",
                        "",
                        "Gerente",
                        "Gerência",
                        true,
                        true,
                        new DateTime(2025, 2, 8, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "Funcionário Teste",
                        "funcionario@cozinha.com",
                        "22222222222",
                        "",
                        "Funcionário",
                        "Salão",
                        true,
                        false,
                        new DateTime(2025, 2, 8, 0, 0, 0, DateTimeKind.Utc)
                    }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Funcionarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Funcionarios",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
