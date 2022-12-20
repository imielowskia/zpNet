using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace zpnet.Models;
[Table("Grades")]
[PrimaryKey(nameof(StudentId), nameof(CourseId))]
public class Grade
{
    public int StudentId{get;set;}
    public int CourseId{get;set;}
    [Range(0, 5.5)]
    [RegularExpression("2|3|4|5")]
    public decimal Ocena{get;set;}
    [ForeignKey("StudentId")]
    public Student Student{get;set;}
    [ForeignKey("CourseId")]
    public Course Course{get;set;}
}