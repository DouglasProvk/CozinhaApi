using CozinhaApi.Models;
using CozinhaAPI.Data.DataConfiguration;
using Microsoft.EntityFrameworkCore;

namespace CozinhaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Prato> Pratos { get; set; } // injeção de dependência para acessar a tabela Pratos
        public DbSet<Reserva> Reservas { get; set; } // injeção de dependência para acessar a tabela Reservas

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PratosConfiguration());
            modelBuilder.ApplyConfiguration(new ReservasConfiguration());
        }
    }
}