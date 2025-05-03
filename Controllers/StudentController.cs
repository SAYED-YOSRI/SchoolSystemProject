using final_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace final_project.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Profile()
        {

            var student = _context.Students
                .Include(s => s.Enrollments)
                .Include(s => s.Notifications)
                .FirstOrDefault(s => s.Id == 1);

            return View(student);
        }
        public IActionResult Courses()
        {
            var studentId = 1;

            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToList();

            return View(enrollments);
        }
        public IActionResult Grades()
        {
            int studentId = 1;

            var results = _context.ExamResults
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Course)
                .Include(r => r.Enrollment)
                .Where(r => r.Enrollment.StudentId == studentId)
                .ToList();

            return View(results);
        }
        public IActionResult ExamSchedule()
        {
            int studentId = 1;

            var exams = _context.Exams
                .Include(e => e.Course)
                .Where(e => e.Course.Enrollments.Any(en => en.StudentId == studentId))
                .ToList();

            return View(exams);
        }

        public IActionResult Timetable()
        {
            int studentId = 1;

            var timetable = _context.Timetables
                .Include(t => t.Course)
                .Where(t => t.StudentId == studentId)
                .ToList();

            return View(timetable);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == 1);
            return View(student);
        }

        [HttpPost]
        public IActionResult EditProfile(Student updatedStudent)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == updatedStudent.Id);

            if (student != null)
            {
                student.FullName = updatedStudent.FullName;
                student.phoneNumber = updatedStudent.phoneNumber;
                student.addres = updatedStudent.addres;


                _context.SaveChanges();
                return RedirectToAction("Profile");
            }

            return View(updatedStudent);
        }
        public IActionResult Notifications()
        {
            int studentId = 1;

            var notifications = _context.NotificationRecipients
                .Include(nr => nr.Notification)
                .ThenInclude(n => n.Admin)
                .Where(nr => nr.StudentId == studentId)
                .Select(nr => nr.Notification)
                .OrderByDescending(n => n.SentAt)
                .ToList();

            return View(notifications);
        }
    }
}
