using HabitAqui.Models;
using System.ComponentModel.DataAnnotations;

namespace HabitAqui.ViewModels
{
    public class HabitacoesViewModel
    {
        public List<Habitacao> ListaHabitacoes;

        public int NResults { get; set; }

        [Display(Name = "Texto", Prompt = "introduza o texto a pesquisar")]
        public string? TextoAPesquisar { get; set; }
        public bool Disponivel { get; set; }
        public string? Localizacao { get; set; } 
        public string? Tipologia { get; set; }
        public int Ordenar { get; set; } // isto serve para escolher qual é o Order By que o user vai escolher
        public DateTime DataInicioPesquisa { get; set; }
        public DateTime DataFinalPesquisa { get; set; }
    }
}
