using Humanizer.Bytes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HabitAqui.Models
{
    public class Habitacao
    {
        public int Id { get; set; }

        [Display(Name = "Nome", Prompt = "Qual o nome da habitação?")]
        public string Nome { get; set; }

        [Display(Name = "Tipo", Prompt = "Qual é o tipo da habitação?")]
        public string Tipo { get; set; }
        public int IdContrato { get; set; }
        
        //[Display(Name = "Contrato", Prompt = "Contrato associado à habitação")]
        //public Contrato Contrato { get; set; }

        public int IdCategoria { get; set; }

        //[Display(Name = "Categoria", Prompt = "Qual a categoria desta habitação?")]
        //public Categoria Categoria { get; set; }

        [Display(Name = "Disponível", Prompt = "Esta habitação irá estar disponível?")]
        public bool Disponivel { get; set; }

        [Display(Name = "Localização", Prompt = "Qual é a localização desta habitação?")]
        public string Localizacao { get; set; }

        [Display(Name = "Número de Casas de Banho", Prompt = "Quantas casas de banho tem esta habitação?")]
        public int NBath { get; set; }

        [Display(Name = "Quartos", Prompt = "Quantos quartos tem a habitação?")]
        public int NBedroom { get; set; }

        [Display(Name = "Área", Prompt = "Qual é a área (em metros quadrados) desta habitação?")]
        public decimal? Area { get; set; }

<<<<<<< Updated upstream
        public int IdArrendamento { get; set; }

        //[Display(Name = "Arrendamento", Prompt = "Arrendamento desta habitação")]
        //public Arrendamento Arrendamento { get; set; }
=======
        public TipoHabitacao TipoHabitacao { get; set; } // se é apartamento ou casa ou bla bla bla
>>>>>>> Stashed changes

        public int IdLocador { get; set; }

        //[Display(Name = "Locador", Prompt = "Quem é o Locador desta habitação?")]
        //public Locador Locador { get; set; }

        [Display(Name = "Avaliação", Prompt = "As habitações não podem ter avaliações quando criadas, mudar depois")]
        public string Avaliacao { get; set; }

<<<<<<< Updated upstream
        [Display(Name = "Estado da Habitação", Prompt = "Como se encontra a habitação?")]
        public string Estado { get; set; } // novo, renovado e usado
=======
        public int IdEstado { get; set; }

        public Estado Estado { get; set; } // novo, renovado e usado
>>>>>>> Stashed changes

        [Display(Name = "Danos", Prompt = "A habitação possui alguns danos?")]
        public string Danos { get; set; }

        [Display(Name = "Observações", Prompt = "Deixe umas observações sobre a habitação")]
        public string Observacoes { get; set; }

        public Tipologia Tipologia { get; set; }

        public int IdTipologia { get; set; }

        public string Image { get; set; }
    }
}