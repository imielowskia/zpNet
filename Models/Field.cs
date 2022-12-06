using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace zpnet.Models
{
    [Table("Fields")]
    public class Field
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Nazwa { get; set; }
        public string? Opis { get; set; }
        
        public ICollection<Student>? Students { get; set; }
    }
}
