using Humanizer.Bytes;

namespace HabitAqui.Models
{
    public class Habitacao
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Tipo { get; set; }

        public int IdContrato { get; set; }

        //public Contrato Contrato { get; set; }

        public int IdCategoria { get; set; }

        //public Categoria Categoria { get; set; }

        public bool Disponivel { get; set; }

        public string Localizacao { get; set; }

        public int NBath { get; set; }

        public int NBedroom { get; set; }

        public decimal? Area { get; set; }

        public int IdArrendamento { get; set; }

        //public Arrendamento Arrendamento { get; set; }

        public int IdLocador { get; set; }

        //public Locador Locador { get; set; }

        public string Avaliacao { get; set; }

        public string Estado { get; set; } // novo, renovado e usado

        public string Danos { get; set; }

        public string Observacoes { get; set; }

        public string Image { get; set; }
    }
}