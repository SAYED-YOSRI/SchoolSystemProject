using final_project.Models;
using final_project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace final_project.Controllers
{
    public class AdminController : Controller
    {

        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult ShowAllStudents()
        {
            StudentTeacherListsVM viewModel = new StudentTeacherListsVM()
            {
                Students = context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .ToList(),

                Teachers = context.Teachers.ToList()
            };

            return View("ShowAllStudents", viewModel);
        }

        public IActionResult createStudent ()
        {
            return View("createStudent");
        }

        public IActionResult saveCreate(Student student) 
        {
            if (student != null)
            {
                var exist = context.Students.Any(s=> s.Email == student.Email);

                if (exist) 
                {
                    ModelState.AddModelError("Email", "This Email is Already Exist");

                    return View("createStudent", student);
                }
                else 
                {
                    context.Students.Add(student);
                    context.SaveChanges();
                }
                
            }

            return RedirectToAction("ShowAllStudents");
        
        }
        [HttpPost]
        public IActionResult DeleteStudent(int studentId)
        {
            var student = context.Students.Find(studentId);

            if (student == null)
            {
                return NotFound();
            }

            context.Students.Remove(student);
            context.SaveChanges();
            return RedirectToAction("ShowAllStudents");
        }

        public IActionResult creatTeacher()
        {
            var courses = context.Courses
        .Select(c => new SelectListItem
        {
            Value = c.CourseId.ToString(),
            Text = c.Title
        }).ToList();

            ViewBag.Courses = courses;

            return View("creatTeacher");
        }

        [HttpPost]
        public IActionResult saveCreateTeacher(Teacher teacher, List<int> SelectedCourseIds)
        {
            if (teacher != null)
            {
                var exists = context.Teachers.Any(t => t.Email == teacher.Email);

                if (exists)
                {
                    ModelState.AddModelError("Email", "This Email is Already Exist");

                    ViewBag.Courses = context.Courses
                        .Select(c => new SelectListItem
                        {
                            Value = c.CourseId.ToString(),
                            Text = c.Title
                        }).ToList();

                    return View("creatTeacher", teacher);
                }

                
                if (SelectedCourseIds != null && SelectedCourseIds.Any())
                {
                    var selectedCourses = context.Courses
                        .Where(c => SelectedCourseIds.Contains(c.CourseId))
                        .ToList();

                    teacher.Courses = selectedCourses;
                }

                context.Teachers.Add(teacher);
                context.SaveChanges();
            }

            return RedirectToAction("ShowAllStudents");
        }


        public IActionResult DeleteTeacher(int Id)
        {
            var teacher = context.Teachers
                .Include(t => t.Courses)
                .FirstOrDefault(t => t.Id == Id);

            if (teacher == null)
                return NotFound();

           
            foreach (var course in teacher.Courses)
            {
                course.TeacherId = null;
            }

            context.Teachers.Remove(teacher);
            context.SaveChanges();

            return RedirectToAction("ShowAllStudents");
        }





    }
}
