using GR1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GR1.Controllers;

[Authorize]
public class ClassController : Controller
{
    private readonly AppDbContext _context;

    public ClassController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Lấy ID của giáo viên đã đăng nhập
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // Lấy danh sách lớp học của giáo viên
        var classes = await _context.Classes
            .Where(c => c.TeacherId == userId)
            .Include(c => c.Teacher)
            .ToListAsync();

        return View(classes);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ClassModel classModel)
    {
        if (ModelState.IsValid)
        {
            // Gán ID của giáo viên đã đăng nhập
            classModel.TeacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Kiểm tra thời gian hợp lệ
            if (classModel.StartTime >= classModel.EndTime)
            {
                ModelState.AddModelError("", "Thời gian bắt đầu phải trước thời gian kết thúc.");
                return View(classModel);
            }

            _context.Classes.Add(classModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View(classModel);
        }
        return View(classModel);
    }
    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var classModel = await _context.Classes
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (classModel == null)
        {
            return NotFound();
        }

        // Kiểm tra quyền: chỉ giáo viên chủ lớp mới xem được
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (classModel.TeacherId != userId)
        {
            return Forbid();
        }
        // Lấy RoomCode từ RoomModel nếu cần
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomCode == classModel.RoomCode);
        int? roomCode = room?.RoomCode;

        // Lấy các log phù hợp
        var logs = await _context.Logs
            .Include(l => l.Student)
            .Include(l => l.Room)
            .Where(l =>
                l.Timestamp >= classModel.StartTime &&
                l.Timestamp <= classModel.EndTime &&
                l.RoomCode == roomCode)
            .ToListAsync();

        ViewBag.Logs = logs;
        return View(classModel);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var classModel = await _context.Classes.FindAsync(id);
        if (classModel == null)
        {
            return NotFound();
        }

        // Kiểm tra quyền: chỉ giáo viên chủ lớp mới xóa được
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (classModel.TeacherId != userId)
        {
            return Forbid();
        }

        _context.Classes.Remove(classModel);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    

    
}