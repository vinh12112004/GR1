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
}