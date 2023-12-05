using Humanizer.Bytes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace HabitAqui.Models
{
    public class Habitacao
    {
        public int Id { get; set; }

        [Display(Name = "Nome", Prompt = "Qual o nome da habitação?")]
        public string Nome { get; set; }

        [Display(Name = "Tipo", Prompt = "Qual é o tipo da habitação?")]
        public string? Tipo { get; set; }
        public int? ContratoId { get; set; }
        
        [Display(Name = "Contrato", Prompt = "Contrato associado à habitação")]
        public Contrato? Contrato { get; set; }

        public int? CategoriaId { get; set; }

        [Display(Name = "Categoria", Prompt = "Qual a categoria desta habitação?")]
        public Categoria? Categoria { get; set; }

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

        public int? ArrendamentoId { get; set; }

        //[Display(Name = "Arrendamento", Prompt = "Arrendamento desta habitação")]
        //public Arrendamento Arrendamento { get; set; }

        public int? LocadorId { get; set; }

        [Display(Name = "Locador", Prompt = "Quem é o Locador desta habitação?")]
        public Locador? Locador { get; set; }

        [Display(Name = "Avaliação", Prompt = "As habitações não podem ter avaliações quando criadas, mudar depois")]
        public string Avaliacao { get; set; }
       
        public int? EstadoId { get; set; }

        [Display(Name = "Estado da Habitação", Prompt = "Como se encontra a habitação?")]
        public Estado? Estado { get; set; }

        public int? TipologiaId { get; set; }

        public Tipologia? Tipologia { get; set; }

        public string? Image { get; set; }
    }
}