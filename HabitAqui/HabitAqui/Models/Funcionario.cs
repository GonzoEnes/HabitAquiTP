namespace HabitAqui.Models
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Empresa empresa { get; set; }
        public ApplicationUser applicationUser { get; set; }
    }
}
