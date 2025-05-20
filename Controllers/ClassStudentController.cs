using System.Security.Claims;
using GR1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GR1.Controllers;

public class ClassStudentController : Controller
{
    private readonly AppDbContext _context;

    public ClassStudentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int id)
    {
        var classModel = await _context.Classes
            .FirstOrDefaultAsync(c => c.Id == id);
        // Kiểm tra quyền: chỉ giáo viên chủ lớp mới xem được
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (classModel.TeacherId != userId)
        {
            return Forbid();
        }
        var studentsInClass = await _context.StudentClasses
            .Include(sc => sc.Student)
            .Where(sc => sc.ClassId == id)
            .ToListAsync();
        ViewBag.id = id;
        return View(studentsInClass);
    }
    [HttpPost]
    public async Task<IActionResult> Create(int id, long studentCode)
    {
        var classModel = await _context.Classes.FindAsync(id);

        // Kiểm tra quyền: chỉ giáo viên chủ lớp mới thêm sinh viên
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (classModel.TeacherId != userId)
        {
            return Forbid();
        }

        var studentClass = new StudentClassModel
        {
            ClassId = classModel.Id,
            StudentCode = studentCode
        };

        _context.StudentClasses.Add(studentClass);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", new { id = classModel.Id });
    }

    [HttpGet]
    public IActionResult Create(int id)
    {
        ViewBag.id = id;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, long studentCode)
    {
        var studentClass = await _context.StudentClasses
            .FirstOrDefaultAsync(sc => sc.ClassId == id && sc.StudentCode == studentCode);        // Kiểm tra quyền: chỉ giáo viên chủ lớp mới xóa được
        var classModel = await _context.Classes.FindAsync(studentClass.ClassId);
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (classModel.TeacherId != userId)
        {
            return Forbid();
        }
        _context.StudentClasses.Remove(studentClass);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", new { id = classModel.Id });
    }
}