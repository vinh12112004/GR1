using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GR1.Models;

public class ClassModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ClassName { get; set; } // Tên lớp học

    [Required]
    [ForeignKey("Teacher")]
    public int TeacherId { get; set; } // Khóa ngoại liên kết với UserModel

    [Required]
    [MaxLength(50)]
    public string Room { get; set; } // Phòng học

    [Required]
    public DateTime StartTime { get; set; } // Thời gian bắt đầu

    [Required]
    public DateTime EndTime { get; set; } // Thời gian kết thúc
    [ValidateNever]
    public UserModel? Teacher { get; set; } // Navigation property
}