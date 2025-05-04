using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GR1.Models;
public class LogModel
{
    [Key]
    public int ID { get; set; }
    
    [ForeignKey("Student")]
    [Display(Name = "Mã sinh viên")]
    public long StudentCode { get; set; }
    
    [ForeignKey("Room")]
    [Display(Name = "Mã phòng")]
    public int RoomCode { get; set; }

    [Display(Name = "Trạng thái")]
    public string Status { get; set; }   // "IN" / "OUT"

    [Display(Name = "Thời gian")]
    public DateTime Timestamp { get; set; }

    public StudentModel Student { get; set; }
    public RoomModel Room { get; set; }
}
