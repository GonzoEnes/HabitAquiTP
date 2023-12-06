using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HabitAqui.Models
{
    public class Tipologia // se é casa, apartamento, quarto...
    {
        public int Id { get; set; }

        [Display(Name = "Tipo", Prompt = "Escolha o tipo de habitação")]
        public string Nome { get; set; }
    }
}
