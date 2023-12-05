using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace HabitAqui.Models
{ 
    // estado da habitação
    public class Estado
    {
        public int Id { get; set; }

        [Display(Name = "Estado", Prompt = "Em que estado se encontra esta habitação?")]
        public string Nome { get; set; } // o estado

        [Display(Name = "Equipamentos", Prompt = "Que equipamentos contém esta habitação?")]
        public string? Equipamentos { get; set; }

        [Display(Name = "Danos", Prompt = "A habitação possui algum tipo de danos?")]
        public string? Danos { get; set; }

        [Display(Name = "Observações", Prompt = "Adicione mais algumas observações a ter em conta...")]
        public string? Observacoes { get; set; }
        //public ApplicationUser? ApplicationUser { get; set; }
        
        //public string ApplicationUserId { get; set; }

        [NotMapped]
        [Display(Name = "Fotografias", Prompt = "Anexe fotografias do dano da habitação")]
        public IFormFile[]? Fotografias { get; set; }
    }
}