using CozinhaApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CozinhaAPI.Data.DataConfiguration
{
    public class ReservasConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.NomePessoa)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(r => r.CPFFuncionario)
                .HasMaxLength(14);

            builder.Property(r => r.PratoId)
                .IsRequired();

            builder.Property(r => r.PratoNome)
                .HasMaxLength(150);

            builder.Property(r => r.DataRefeicao)
                .IsRequired();

            builder.Property(r => r.Periodo)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(r => r.Observacoes)
                .HasMaxLength(500);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("confirmada");

            builder.Property(r => r.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(r => r.Prato)
                .WithMany()
                .HasForeignKey(r => r.PratoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice para buscar reservas por data e CPF do funcionário
            builder.HasIndex(r => new { r.DataRefeicao, r.CPFFuncionario });

            builder.ToTable("Reservas");
        }
    }
}