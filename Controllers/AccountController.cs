using final_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace final_project.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check if Admin
            var admin = _context.Admins.FirstOrDefault(a => a.Email == model.Email && a.password == model.Password);

            if (admin != null)
            {
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetInt32("AdminId", admin.AdminId);
                return RedirectToAction("Index", "Admin");
            }

            // Check if Teacher
            var teacher = _context.Teachers.FirstOrDefault(t => t.Email == model.Email && t.Password == model.Password);
            if (teacher != null)
            {
                HttpContext.Session.SetString("Role", "Teacher");
                HttpContext.Session.SetInt32("TeacherId", teacher.TeacherId);
                return RedirectToAction("Index", "Teacher");
            }

            // Check if Student
            var student = _context.Students.FirstOrDefault(s => s.Email == model.Email && s.Password == model.Password);
            if (student != null)
            {
                HttpContext.Session.SetString("Role", "Student");
                HttpContext.Session.SetInt32("StudentId", student.StudentId);
                return RedirectToAction("Index", "Student");
            }

            ViewBag.Error = "Invalid username or password.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
