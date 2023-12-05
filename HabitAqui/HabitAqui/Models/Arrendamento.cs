using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models
{

    // completar, se preciso
    public class Arrendamento
    {
        public int Id { get; set; }

        [Display(Name = "Custo do Arrendamento", Prompt = "Quanto vai custar?")]
        public decimal? CustoArrendamento { get; set; }

        [Display(Name = "Período de Arrendamento", Prompt = "Quantos dias irá ser arrendada a habitação?")]
        public int DiasPeriodoArrendamento { get; set; }
    }
}
