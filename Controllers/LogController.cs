using GR1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GR1.Controllers;
[Authorize]
public class LogController : Controller{
    private readonly AppDbContext _context;
    public LogController(AppDbContext context){
        _context = context;
    }

    // public async Task<IActionResult> Index(){
    //     var listlog = await _context.Logs.ToListAsync();
    //     return View("/Views/Log/Log.cshtml",listlog);
    // }

    [HttpGet]
    public async Task<IActionResult> Index(String StudentCode){
        var logs = from s in _context.Logs select s;
        try
        {
            if(!string.IsNullOrEmpty(StudentCode)){
                logs = logs.Where(s=> s.StudentCode.ToString().Contains(StudentCode));
                if (logs != null)
                {
                    return View("/Views/Log/Log.cshtml" ,await logs.Take(10).ToListAsync());
                }
            }
            ViewBag.StudentCode = StudentCode;
            var allLogs = await logs.ToListAsync();
            return View("/Views/Log/Log.cshtml",allLogs);
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Lỗi khi Search dữ liệu: " + ex.Message);
            return RedirectToAction("Index");
        }
        
    }

    [HttpPost]
    [Route("api/log")]
    [IgnoreAntiforgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> ReceiveLog([FromBody] LogDTO log)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var newLog = new LogModel
        {
            StudentCode = log.StudentCode,
            RoomCode = log.RoomCode,
            Status = log.Status,
            Timestamp = DateTime.Now
        };
        var studentExists = await _context.Students.AnyAsync(s => s.StudentCode == log.StudentCode);
        if (!studentExists)
        {
            // Có thể thông báo lỗi cho người dùng hoặc thêm mới sinh viên
            return BadRequest("StudentCode không tồn tại trong hệ thống.");
        }
        try
        {
            _context.Logs.Add(newLog);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Log ghi thành công" });
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Lỗi khi update dữ liệu: " + ex.Message);
            return BadRequest();
        }



    }
}

public class LogDTO
{
     public long StudentCode { get; set; }
    public int RoomCode { get; set; }
    public string Status { get; set; }
}