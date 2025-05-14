using GR1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GR1.Controllers;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng.";
            return View();
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        return RedirectToAction("Index", "Room");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string fullname ,string email, string password)
    {
        // Kiểm tra thông tin đăng ký (giả sử hardcode, bạn nên thay bằng DB)
        if (string.IsNullOrWhiteSpace(fullname))
        {
            ViewBag.Error = "Họ và tên không được để trống.";
            return View();
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            ViewBag.Error = "Email không được để trống.";
            return View();
        }
        if (_context.Users.Any(u=> u.Email == email ))
        {
            ViewBag.Error = "Tên đăng ký đã tồn tại.";
            return View();
        }
        var passwordHash = HashPassword(password);
        var newUser = new UserModel
        {   
            FullName = fullname,
            Email = email,
            PasswordHash = passwordHash
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        // Lưu thông tin người dùng vào DB (giả sử thành công)
        return RedirectToAction("Login");
    }

    private string HashPassword(string password)
    {
        // Sử dụng một thuật toán băm mạnh mẽ để mã hóa mật khẩu
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var hash = HashPassword(password);
        return hash == storedHash;
    }
}