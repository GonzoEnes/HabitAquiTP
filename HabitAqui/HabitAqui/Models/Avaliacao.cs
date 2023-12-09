using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        [Range(0.00, 5.00, ErrorMessage = "A avaliação deve ser de 0 a 5 estrelas, permitindo valores intermédios (ex: 4.52)")]

        [Display(Name = "Nota de Avaliação", Prompt = "Insira a avaliação dessa habitação")]
        public decimal AvaliacaoNota { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        [Display(Name = "Habitação")]
        public int HabitacaoId { get; set; }
        public Habitacao? Habitacao { get; set; }

    }
}
