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

        [Display(Name = "Custo", Prompt = "Insira o custo da habitação")]
        public decimal? Custo { get; set; }
       
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

        public ICollection<Arrendamento>? Arrendamentos { get; set; }

        public ICollection<Avaliacao>? Avaliacoes { get; set; }

        [Range(0.00, 5.00, ErrorMessage = "A avaliação deve ser de 0 a 5, permitindo valores intermédios (ex: 4,52)")]
        public decimal? MediaAvaliacoes { get; set; }

        public int? EstadoId { get; set; }

        [Display(Name = "Estado da Habitação", Prompt = "Como se encontra a habitação?")]
        public Estado? Estado { get; set; }

        public int? TipologiaId { get; set; }

        [Display(Name = "Tipo de Habitação", Prompt = "Escolha o tipo de habitação")]
        public Tipologia? Tipologia { get; set; }

        public string? Image { get; set; }

        //[Display(Name = "Fotografias da Habitação", Prompt = "Escolha as fotografias a anexar")]
        //public byte[]? Fotografias { get; set; }
    }
}