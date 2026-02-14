using CozinhaApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CozinhaAPI.Data.DataConfiguration
{
    public class FuncionariosConfiguration : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(f => f.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(f => f.CPF)
                .IsRequired()
                .HasMaxLength(14);

            builder.Property(f => f.Telefone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(f => f.Cargo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Departamento)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Ativo)
                .HasDefaultValue(true);

            builder.Property(f => f.IsAdmin)
                .HasDefaultValue(false);

            builder.Property(f => f.DataAdmissao)
                .IsRequired();

            builder.Property(f => f.Observacoes)
                .HasMaxLength(500);

            builder.Property(f => f.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(f => f.UpdatedDate)
                .IsRequired(false);

            // Índice único para CPF
            builder.HasIndex(f => f.CPF)
                .IsUnique();

            builder.ToTable("Funcionarios");
        }
    }
}
