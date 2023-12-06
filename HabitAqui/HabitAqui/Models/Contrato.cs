using System.ComponentModel.DataAnnotations;

namespace HabitAqui.Models
{
    public class Contrato
    {
        public int Id { get; set; }

        [Display(Name = "Contrato", Prompt = "Contrato")]
        public string Nome { get; set; }

        [Display(Name = "Data Início do Contrato", Prompt = "Quando terá início o contrato?")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data Final do Contrato", Prompt = "Quando finalizará este contrato?")]
        public DateTime DataFim { get; set; }
    }
}
