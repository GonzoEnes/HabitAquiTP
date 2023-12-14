using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitAqui.Models
{
    [Table("Avaliacao")]
    public class Avaliacao
    {
        public int Id { get; set; }
        public int Nota { get; set; }

    }
}
