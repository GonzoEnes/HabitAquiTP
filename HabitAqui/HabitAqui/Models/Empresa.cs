using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HabitAqui.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        [Display(Name = "Nome", Prompt = "Insira o nome da empresa")]
        public string Nome { get; set; }

        [Display(Name = "Avaliação", Prompt = "Insira a avaliação desta empresa")]
        public int Avaliacao { get; set; }

        public ICollection<Habitacao>? habitacaos { get; set; }
        public ICollection<Funcionario>? funcionarios { get; set; }
        public ICollection<Gestor>? gestores { get; set; }
    }
}
