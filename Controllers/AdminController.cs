using final_project.Models;
using final_project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace final_project.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ShowAllStudents()
        {
            List<Student> students = _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .ToList();
           

            return View("ShowAllStudents", students);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ShowAllTeachers()
        {
            List<Teacher> teachers = _context.Teachers
                .Include(t => t.Courses)
                .ThenInclude(c => c.Department)
                .ToList();
            return View("ShowAllTeachers", teachers);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult createStudent()
        {
            return View("createStudent");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult saveCreate(Student student)
        {
            if (student != null)
            {
                var exist = _context.Students.Any(s => s.Email == student.Email);

                if (exist)
                {
                    ModelState.AddModelError("Email", "This Email is Already Exist");

                    return View("createStudent", student);
                }
                else
                {
                    _context.Students.Add(student);
                    _context.SaveChanges();
                }

            }

            return RedirectToAction("ShowAllStudents");

        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteStudent(int studentId)
        {
            var student = _context.Students.Find(studentId);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            _context.SaveChanges();
            return RedirectToAction("ShowAllStudents");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditStudent(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();

            return View(student);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditStudent(Student updatedStudent)
        {
            if (updatedStudent != null)
            {
                var email = _context.Students.Select(s => s.Email).FirstOrDefault();
                if (_context.Students.Any(s => s.Email == updatedStudent.Email) && updatedStudent.Email != email )
                {
                    ModelState.AddModelError("Email", "The email is already in use by another student.");
                    return View(updatedStudent);
                }


                var student = _context.Students.FirstOrDefault(s => s.Id == updatedStudent.Id);
                if (student == null)
                    return NotFound();

                student.FullName = updatedStudent.FullName;
                student.Email = updatedStudent.Email;
                student.password = updatedStudent.password;
                student.phoneNumber = updatedStudent.phoneNumber;
                student.addres = updatedStudent.addres;

                _context.SaveChanges();
            }
            
            return RedirectToAction("ShowAllStudents");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ShowStudentCourses(int id)
        {
            var student = _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .ThenInclude(c => c.Teacher) // Include the Teacher navigation property
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var courses = student.Enrollments.Select(e => e.Course).ToList();
            return View(courses);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ViewEnrolledStudents(int courseId)
        {
            var course = _context.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return NotFound();

            return View("ViewEnrolledStudents", course);


        }
        [Authorize(Roles = "Admin")]
        public IActionResult creatTeacher()
        {
            var courses = _context.Courses
        .Select(c => new SelectListItem
        {
            Value = c.CourseId.ToString(),
            Text = c.Title
        }).ToList();

            ViewBag.Courses = courses;

            return View("creatTeacher");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult saveCreateTeacher(Teacher teacher, List<int> SelectedCourseIds)
        {
            if (teacher != null)
            {
                var email = _context.Teachers.Select(s => s.Email).FirstOrDefault();
                var exists = _context.Teachers.Any(t => t.Email == teacher.Email);

                if (exists && teacher.Email != email)
                {
                    ModelState.AddModelError("Email", "This Email is Already Exist");

                    ViewBag.Courses = _context.Courses
                        .Select(c => new SelectListItem
                        {
                            Value = c.CourseId.ToString(),
                            Text = c.Title
                        }).ToList();

                    return View("creatTeacher", teacher);
                }


                if (SelectedCourseIds != null && SelectedCourseIds.Any())
                {
                    var selectedCourses = _context.Courses
                        .Where(c => SelectedCourseIds.Contains(c.CourseId))
                        .ToList();

                    teacher.Courses = selectedCourses;
                }

                _context.Teachers.Add(teacher);
                _context.SaveChanges();
            }

            return RedirectToAction("ShowAllTeachers");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ShowTeacherCourses(int id)
        {
            var teacher = _context.Teachers
                .Include(t => t.Courses)
                .FirstOrDefault(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            var courses = teacher.Courses.ToList();
            return View(courses);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteTeacher(int Id)
        {
            var teacher = _context.Teachers
                .Include(t => t.Courses)
                .FirstOrDefault(t => t.Id == Id);

            if (teacher == null)
                return NotFound();


            foreach (var course in teacher.Courses)
            {
                course.TeacherId = null;
            }

            _context.Teachers.Remove(teacher);
            _context.SaveChanges();

            return RedirectToAction("ShowAllTeachers");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditTeacher(int id)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            return View("EditTeacher", teacher);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditTeacher(Teacher updatedTeacher)
        {
            
            if (updatedTeacher != null)
            {
                if (_context.Students.Any(s => s.Email == updatedTeacher.Email))
                {
                    ModelState.AddModelError("Email", "The email is already in use by another student.");
                    return View(updatedTeacher);
                }

                var teacher = _context.Teachers.FirstOrDefault(t => t.Id == updatedTeacher.Id);
                if (teacher == null)
                    return NotFound();

                teacher.FullName = updatedTeacher.FullName;
                teacher.Email = updatedTeacher.Email;
                teacher.password = updatedTeacher.password;
                teacher.phoneNumber = updatedTeacher.phoneNumber;
                teacher.addres = updatedTeacher.addres;

                _context.SaveChanges();
            }

            return RedirectToAction("ShowAllTeachers");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ShowAllCourses()
        {
            List<Course> courses = _context.Courses
             .Include(c => c.Enrollments)
             .ThenInclude(e => e.Student)
             .Include(t => t.Teacher)
             .Include(d => d.Department)
             .ToList();

            return View("ShowAllCourses", courses);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCourse()
        {
            ViewBag.Teachers = _context.Teachers
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.FullName
                }).ToList();

            ViewBag.Departments = _context.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }).ToList();

            return View("CreateCourse");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult SaveCreateCourse(Course course)
        {


            if (course != null)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction("ShowAllCourses");
            }



            ViewBag.Teachers = _context.Teachers
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.FullName
                }).ToList();

            ViewBag.Departments = _context.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }).ToList();

            return RedirectToAction("ShowAllCourses", course);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _context.Courses
            .Include(t => t.Teacher).
             Include(d => d.Department)
            .FirstOrDefault(t => t.CourseId == id);

            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("ShowAllCourses");

        }
        // Timetable
        [Authorize(Roles = "Admin")]
        public IActionResult ShowTimetable()
        {
            var grouped = _context.Timetables
                .Include(t => t.Course)
                .Include(t => t.Teacher)
                .ToList()
                .GroupBy(t => new { t.CourseId, t.TeacherId })
                .Select(g => new TimetableViewModel
                {
                    CourseTitle = g.First().Course.Title,
                    TeacherName = g.First().Teacher.FullName,
                    Schedule = g
                        .GroupBy(x => x.Day)
                        .ToDictionary(
                            d => d.Key,
                            d => d.Select(x => $"{x.StartTime:hh\\:mm} - {x.EndTime:hh\\:mm} at {x.Location}").ToList()
                        ),
                    EntryIds = g
                        .GroupBy(x => x.Day)
                        .ToDictionary(
                            d => d.Key,
                            d => d.Select(x => x.Id).ToList()
                        )
                }).ToList();

            return View(grouped);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditTimetable(int id)
        {
            var timetable = _context.Timetables.Find(id);
            if (timetable == null)
            {
                return NotFound();
            }

            ViewBag.Courses = _context.Courses
                .Select(c => new SelectListItem { Value = c.CourseId.ToString(), Text = c.Title })
                .ToList();

            ViewBag.Teachers = _context.Teachers
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.FullName })
                .ToList();

            return View(timetable);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditTimetable(Timetable entry)
        {
            if (entry == null )
            {
                // Populate ViewBag.Courses and ViewBag.Teachers when ModelState is invalid
                ViewBag.Courses = _context.Courses
                    .Select(c => new SelectListItem { Value = c.CourseId.ToString(), Text = c.Title })
                    .ToList();

                ViewBag.Teachers = _context.Teachers
                    .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.FullName })
                    .ToList();

                return View("EditTimetable", entry);
            }

            // Update the timetable entry
            var existingEntry = _context.Timetables.Find(entry.Id);
            if (existingEntry == null)
            {
                return NotFound();
            }

            existingEntry.CourseId = entry.CourseId;
            existingEntry.TeacherId = entry.TeacherId;
            existingEntry.Day = entry.Day;
            existingEntry.StartTime = entry.StartTime;
            existingEntry.EndTime = entry.EndTime;
            existingEntry.Location = entry.Location;

            _context.SaveChanges();
            return RedirectToAction("ShowTimetable");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var entry = _context.Timetables.Find(id);
            if (entry == null)
                return NotFound();

            _context.Timetables.Remove(entry);
            _context.SaveChanges();
            return RedirectToAction("ShowTimetable");
        }

        //exam Timetable 
        [Authorize(Roles = "Admin")]
        public IActionResult ShowExamTimetable()
        {
            var exams = _context.Exams.Include(e => e.Course).ToList();

            var grouped = exams
                .GroupBy(e => e.ExamDate.DayOfWeek)
                .Select(g => new ExamScheduleViewModel
                {
                    Day = g.Key,
                    Exams = g.Select(e => new ExamScheduleViewModel.ExamEntry
                    {
                        ExamId = e.ExamId,
                        CourseTitle = e.Course.Title,
                        ExamDate = e.ExamDate,
                        Location = e.Location
                    }).ToList()
                })
                .OrderBy(e => e.Day)
                .ToList();

            return View(grouped);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/EditExam/{id}")]
        public IActionResult EditExam(int id)
        {
            var exam = _context.Exams.Find(id);
            if (exam == null)
            {
                return NotFound();
            }

            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title", exam.CourseId);
            return View("EditExam", exam);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditExam(Exam exam)
        {
            if (exam == null )
            {
                // Populate ViewBag.Courses to ensure the dropdown is available
                ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title", exam?.CourseId);
                return View("EditExam", exam);
            }

            // Find the existing exam entry
            var existingExam = _context.Exams.Find(exam.ExamId);
            if (existingExam == null)
            {
                return NotFound();
            }

            // Update the exam properties
            existingExam.CourseId = exam.CourseId;
            existingExam.ExamDate = exam.ExamDate;
            existingExam.Location = exam.Location;

            // Save changes to the database
            _context.SaveChanges();
            return RedirectToAction("ShowExamTimetable");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateExam()
        {
            // Populate ViewBag.Courses for the dropdown
            ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title");
            return View("CreateExam");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateExam(Exam exam)
        {
            if (exam== null)
            {
                // Repopulate ViewBag.Courses if validation fails
                ViewBag.Courses = new SelectList(_context.Courses.ToList(), "CourseId", "Title");
                return View("CreateExam", exam);
            }

            // Add the new exam to the database
            _context.Exams.Add(exam);
            _context.SaveChanges();

            // Redirect to ShowExamTimetable after successful creation
            return RedirectToAction("ShowExamTimetable");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteExam(int id)
        {
            var exam = _context.Exams.Find(id);
            if (exam == null)
            {
                return NotFound();
            }

            _context.Exams.Remove(exam);
            _context.SaveChanges();

            return RedirectToAction("ShowExamTimetable");
        }

    }
}
