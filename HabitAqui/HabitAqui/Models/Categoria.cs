using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models
{
    public class Categoria // se é T1, T2, T3...
    {
        public int Id { get; set; }

        [Display(Name = "Categoria", Prompt = "Insira o nome da categoria")]
        public string Nome { get; set; }

        [Display(Name = "Descrição", Prompt = "Insira a descrição da categoria")]
        public string Descricao { get; set; }

        public bool Disponivel { get; set; }

        public ICollection<Habitacao>? Habitacoes { get; set; }

    }
}