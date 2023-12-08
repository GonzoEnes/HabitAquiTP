using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        [Range(0.00, 5.00, ErrorMessage = "A avaliação deve ser de 0 a 5 estrelas, permitindo valores intermédios (ex: 4.52)")]
        public decimal Avalicao { get; set; }
        public string AplicationUserId { get; set; }
        public ApplicationUser AplicationUser { get; set; }
        public int HabitacaoId { get; set; }
        public Habitacao? Habitacao { get; set; }    

    }
}
