using CozinhaApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CozinhaAPI.Data.DataConfiguration
{
    public class PratosConfiguration : IEntityTypeConfiguration<Prato>
    {
        public void Configure(EntityTypeBuilder<Prato> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.Descricao)
                .HasMaxLength(500);

            builder.Property(p => p.Categoria)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Disponivel)
                .HasDefaultValue(true);

            builder.Property(p => p.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.ToTable("Pratos");
        }
    }
}