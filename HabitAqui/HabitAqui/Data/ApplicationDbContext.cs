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

        }

        public DbSet<Arrendamento> Arrendamentos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Contrato> Contratos { get; set; }

        public DbSet<Habitacao> Habitacoes { get; set; }

        public DbSet<Locador> Locadores { get; set; }
    }
}