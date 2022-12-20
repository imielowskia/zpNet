using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zpnet.Models;
public class CG
{
    public int StudentId{get;set;}
    public string IN{get;set;}
    [Range(0, 5.5)]
    [RegularExpression("2|3|4|5")]
    public decimal Grade{get;set;}
}