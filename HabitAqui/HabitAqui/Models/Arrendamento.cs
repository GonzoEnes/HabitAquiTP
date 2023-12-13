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

        [Display(Name = "Habitação", Prompt = "Insira a habitação")]
        public int? HabitacaoId { get; set; }

        
        public Habitacao? Habitacao { get; set; }

        [Display(Name = "Utilizador", Prompt = "Utilizador que realizou o arrendamento")]
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser? ApplicationUser { get; set; }

        [Display(Name = "Estado da Entrega", Prompt = "Insira o estado da habitação a entregar")]
        public int? EstadoEntregaId { get; set; }

        public Estado? EstadoEntrega { get; set; }

        [Display(Name = "Estado da Receção", Prompt = "Insira o estado da habitação a receber")]
        public int? EstadoRececaoId { get; set; }

        [Display(Name = "Estado", Prompt = "Insira o estado da habitação aquando recebida")]
        public Estado? EstadoRececao { get; set; }

        public bool? Confirmado { get; set; }
    }
}
