using HabitAqui.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HabitAqui.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Arrendamento> Arrendamentos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Habitacao> Habitacoes { get; set; }

        public DbSet<Tipologia> Tipologia { get; set; }

        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<Funcionario> Funcionarios { get; set; }

        public DbSet<Gestor> Gestores { get; set; }
    }
}