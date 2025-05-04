using GR1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GR1.Controllers;
public class LogController : Controller{
    private readonly AppDbContext _context;
    public LogController(AppDbContext context){
        _context = context;
    }

    public async Task<IActionResult> Index(){
        var listlog = await _context.Logs.ToListAsync();
        return View("/Views/Log/Log.cshtml",listlog);
    }

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
            var top10 = await logs.Take(10).ToListAsync();
            return View("/Views/Log/Log.cshtml",top10);
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Lỗi khi Search dữ liệu: " + ex.Message);
            return RedirectToAction("Index");
        }
        
    }
}