using HabitAqui.Data.Migrations;
using HabitAqui.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HabitAqui.ViewModels
{
    public class ArrendamentosViewModel
    {
        public List<Arrendamento>? ListaArrendamentos { get; set; }

        [Display(Name = "Data de Início", Prompt = "yyyy-mm-dd")]
        public DateTime DataInicio { get; set; }
        [Display(Name = "Data de Término", Prompt = "yyyy-mm-dd")]
        public DateTime DataFinal { get; set; }

        [Display(Name = "Habitação", Prompt = "Escolher a habitação")]
        public int? HabitacaoId { get; set; }

        [Display(Name = "Habitação", Prompt = "Nome da Habitação")]
        public string? HabitacaoNome { get; set; }

        public Habitacao? Habitacao { get; set; }
        public int? CategoriaId { get; set; }

        [Display(Name = "Categoria", Prompt = "Categoria da Habitação")]
        public Categoria? Categoria { get; set; }

        [Display(Name = "Cliente")]
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? Cliente { get; set; }

        public int Ordenar { get; set; }

        
    }
}
