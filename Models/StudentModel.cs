using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GR1.Models;

public class StudentModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "Mã sinh viên")]
    public long StudentCode { get; set; }

    [Required]
    [Display(Name = "Họ và tên")]
    public string FullName { get; set; }
    public ICollection<LogModel> Logs { get; set; }
}
