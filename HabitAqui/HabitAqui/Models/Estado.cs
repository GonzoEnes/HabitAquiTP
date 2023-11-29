namespace HabitAqui.Models
{ 
    // estado da habitação
    public class Estado
    {
        public int Id { get; set; } 

        public string Nome { get; set; } // o estado

        public int IdHabitacao { get; set; }
    }
}