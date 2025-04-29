using final_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace final_project.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult ShowAllStudents()
        {
            var students = context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .ToList();

            return View("ShowAllStudents", students);
        }

        public IActionResult createStudent ()
        {
            return View("createStudent");
        }

        public IActionResult saveCreate(Student student) 
        {
            if (student != null)
            {
                context.Students.Add(student);
                context.SaveChanges();
            }

            return RedirectToAction("ShowAllStudents");
        
        }
    }
}
