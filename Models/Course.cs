using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zpnet.Models;
[Table("Courses")]
public class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id{get;set;}
    [MaxLength(50)]
    [Display(Name ="Przedmiot")]
    [Required]
    public string? Nazwa{get;set;}
    public ICollection<Student>? Students{get;set;}
    public ICollection<Grade>? Grades {get;set;}

}