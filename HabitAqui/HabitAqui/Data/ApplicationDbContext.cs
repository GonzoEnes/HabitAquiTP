using HabitAqui.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // criar aqui os dbSets da BD para conseguir ir buscar info

        }

        public DbSet<Arrendamento> Agendamentos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Contrato> Contratos { get; set; }

        public DbSet<Habitacao> Habitacoes { get; set; }

        public DbSet<Locador> Locadores { get; set; }

        public DbSet<Reserva> Reservas { get; set; }
    }
}