using System.ComponentModel.DataAnnotations;

namespace GR1.Models;

public class UserModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public string PasswordHash { get; set; } // Lưu mật khẩu đã mã hóa

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } // Tên đầy đủ của người dùng
}