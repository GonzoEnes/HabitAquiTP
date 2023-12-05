using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HabitAqui.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Primeiro Nome", Prompt = "Inserir o seu primeiro nome")]
        public string PrimeiroNome { set; get; }
        [Display(Name = "Último Nome", Prompt = "Inserir o seu último nome")]
        public string UltimoNome { set; get; }
        public DateTime DataNascimento { set; get; }
        [Display(Name = "NIF", Prompt = "Inserir o seu NIF")]
        public int NIF { set; get; }
        [Display(Name = "Available")]
        public bool? Disponivel { get; set; }
        public DateTime? DataRegisto { get; set; }
    }
}
