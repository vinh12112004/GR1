using System.ComponentModel.DataAnnotations;

namespace GR1.Models;
public class RoomModel
{
    [Key]

    public int RoomCode { get; set; }
    public ICollection<LogModel> Logs { get; set; }

}