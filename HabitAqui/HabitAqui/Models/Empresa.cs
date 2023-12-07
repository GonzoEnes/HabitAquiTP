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

        [Display(Name = "Disponível")]
        public bool Disponivel { get; set; }

        [Display(Name = "Lista de Habitações")]
        public ICollection<Habitacao> Habitacoes { get; set; }
        public ICollection<Funcionario> Funcionarios { get; set; }
        public ICollection<Gestor> Gestores { get; set; }
    }
}
