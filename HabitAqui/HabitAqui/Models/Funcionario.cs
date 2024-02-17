using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models
{
    public class Funcionario
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int EmpresaId { get; set; }

        public Empresa Empresa { get; set; }

        [Display(Name = "Disponível")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
