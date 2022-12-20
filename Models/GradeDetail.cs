using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zpnet.Models;
[Table("GradeDetails")]
public class GradeDetail
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;set;}
    [Range(0, 5.5)]
    [RegularExpression("2|3|4|5")]
    public decimal Ocena{get;set;}
    [DataType(DataType.Date)]
    public DateTime Data {get;set;}
    public int StudentId{get;set;}
    public int CourseId{get;set;}
    [ForeignKey("StudentId")]
    public Student Student{get;set;}
    [ForeignKey("CourseId")]
    public Course Course{get;set;}
}