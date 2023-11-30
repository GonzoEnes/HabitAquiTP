using HabitAqui.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HabitAqui.ViewModels
{
    public class PesquisaHabitacaoViewModel
    {
        public List<Habitacao>? ListaDeHabitacoes { get; set; }
        public int NumResultados { get; set; }
        [Display(Name = "PESQUISA DE Habitacoes ...", Prompt = "introduza o texto a pesquisar")]
        public string? TextoAPesquisar { get; set; }

    }
}
