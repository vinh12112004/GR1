using System.Threading.Tasks;
using GR1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GR1.Controllers;
public class RoomController : Controller {
    private readonly AppDbContext _context;
    public RoomController(AppDbContext context){
        _context = context;
    }

    public async Task<IActionResult> Index()
{
    var rooms = _context.Rooms.ToList();
    // lay ban log cuoi cung cua sinh vien
    var latestLogs = await _context.Logs
    .GroupBy(log => new { log.RoomCode, log.StudentCode })
    .Select(g => g.OrderByDescending(log => log.Timestamp).FirstOrDefault())
    .ToListAsync(); 

    // lay ra nhung ban log in
    var studentCounts = latestLogs
    .Where(log => log.Status == "IN")
    .GroupBy(log => log.RoomCode)
    .ToDictionary(
        g => g.Key,
        g => g.Select(log => log.StudentCode).Distinct().Count()
    );

     ViewBag.StudentCounts = studentCounts;
    return View(rooms);
}

}