using GR1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GR1.Controllers;
public class StudentController : Controller
{
    private readonly AppDbContext _context;

    public StudentController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _context.Students.Take(10).ToListAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Index(String StudentCodeorFullName){
        var students = from s in _context.Students select s;
        try
        {
            if(!string.IsNullOrEmpty(StudentCodeorFullName)){
                students = students.Where(s=> s.StudentCode.ToString().Contains(StudentCodeorFullName) || s.FullName.Contains(StudentCodeorFullName));
                if (students != null)
                {
                    return View(await students.Take(10).ToListAsync());
                }
            }
            var top10 = await students.Take(10).ToListAsync();
            return View(top10);
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Lỗi khi Search dữ liệu: " + ex.Message);
            return View();
        }
        
    }

    public IActionResult Create() => View();
    
    
    [HttpPost]
    public async Task<IActionResult> Create(StudentModel student)
    {
        if(_context.Students.Any(s => s.StudentCode == student.StudentCode))
        {
            ModelState.AddModelError("StudentCode", "Mã sinh viên đã tồn tại");
            return View(student);
        }
        
        try
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.Message);
            return View(student);
        }
    }

    public async Task<IActionResult> Edit(long id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
    
    [HttpPost]
    public async Task<IActionResult> Edit(long id, StudentModel student)
    {
        
        try
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Lỗi khi update dữ liệu: " + ex.Message);
            return View(student);
        }
    }

    public async Task<IActionResult> Delete(long id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
    
    [HttpPost]
    public async Task<IActionResult> Delete(StudentModel student)
    {
        try
        {   
            var log = await _context.Logs.Where(l=> l.StudentCode == student.StudentCode).FirstOrDefaultAsync();
            if(log != null){
                _context.Logs.Remove(log);    
            }
            
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Lỗi khi Delete dữ liệu: " + ex.Message);
            return View(student);
        }
    }
}
