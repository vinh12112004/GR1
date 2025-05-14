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
}