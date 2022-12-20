using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zpnet.Models
{

    [Table("Students")]
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Numer albumu")]
        [MaxLength(6),MinLength(6)]
        public string? Indeks { get; set; }
        [Display(Name ="Imię")]
        public string? Imie { get; set; }
        
        public string? Nazwisko { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Data urodzenia")]
        public DateTime? Data_u { get; set; }
        public int? FieldId { get; set; }
        [ForeignKey("FieldId")]
        public Field? Field { get; set; }
        public ICollection<Course>? Courses{get;set;}
        public ICollection<Grade>? Grades {get;set;}
        public ICollection<GradeDetail>? GradeDetails{get;set;}
        public string IN
        {
            get
            {
                return Imie + " " + Nazwisko;
            }
        }
    }
}
