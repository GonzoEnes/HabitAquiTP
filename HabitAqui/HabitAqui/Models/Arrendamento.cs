using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models
{
    public class Arrendamento
    {
        public int Id { get; set; }

        [Display(Name = "Custo do Arrendamento", Prompt = "Quanto vai custar?")]
        public decimal CustoArrendamento { get; set; }

        [Display(Name = "Data Início Arrendamento", Prompt = "Insira a data de início do arrendamento")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data Final Arrendamento", Prompt = "Insira a data de finalização do arrendamento")]
        public DateTime DataFinal { get; set; }

        public DateTime DataPedido { get; set; }

        public int? HabitacaoId { get; set; }

        [Display(Name = "Habitação", Prompt = "Insira a habitação")]
        public Habitacao? Habitacao { get; set; }

        public string ApplicationUserId { get; set; }

        [Display(Name = "Utilizador", Prompt = "Utilizador que realizou o arrendamento")]
        public ApplicationUser? ApplicationUser { get; set; }

        public int? EstadoId { get; set; }

        [Display(Name = "Estado", Prompt = "Insira o estado da habitação")]
        public Estado? Estado { get; set; }

        public bool? Confirmado { get; set; }
    }
}
