using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GR1.Models;
public class StudentClassModel
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Student")]
    public long StudentCode { get; set; }
    public StudentModel Student { get; set; }

    [ForeignKey("Class")]
    public int ClassId { get; set; }
    public ClassModel Class { get; set; }
}