namespace HabitAqui.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int IdCliente { get; set; }

        public int IdHabitacao { get; set; }

        public Habitacao Habitacao { get; set; }

        public int IdContrato { get; set; }
    }
}
