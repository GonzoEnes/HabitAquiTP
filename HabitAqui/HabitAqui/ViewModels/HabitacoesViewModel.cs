using HabitAqui.Models;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui.ViewModels
{
    public class HabitacoesViewModel
    {
        public List<Habitacao> ListaHabitacoes;

        public int NResults { get; set; }

        [Display(Name = "Texto", Prompt = "introduza o texto a pesquisar")]
        public string TextoAPesquisar { get; set; }

        public bool Disponivel { get; set; }

        public string Localizacao { get; set; }

        public string Nome { get; set; } // ? 
    }
}
