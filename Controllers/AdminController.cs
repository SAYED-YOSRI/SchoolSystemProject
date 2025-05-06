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

        public IActionResult createStudent()
        {
            return View("createStudent");
        }

        public IActionResult saveCreate(Student student)
        {
            if (student != null)
            {
                var exist = context.Students.Any(s => s.Email == student.Email);

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
        [HttpGet]
        public IActionResult EditStudent(int id)
        {
            var student = context.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();

            return View(student);
        }
        [HttpPost]
        public IActionResult EditStudent(Student updatedStudent)
        {
            if (updatedStudent != null)
            {
                if (context.Students.Any(s => s.Email == updatedStudent.Email))
                {
                    ModelState.AddModelError("Email", "The email is already in use by another student.");
                    return View(updatedStudent);
                }


                var student = context.Students.FirstOrDefault(s => s.Id == updatedStudent.Id);
                if (student == null)
                    return NotFound();

                student.FullName = updatedStudent.FullName;
                student.Email = updatedStudent.Email;
                student.password = updatedStudent.password;
                student.phoneNumber = updatedStudent.phoneNumber;
                student.addres = updatedStudent.addres;

                context.SaveChanges();
            }
            
            return RedirectToAction("ShowAllStudents");
        }
        public IActionResult ShowStudentCourses(int id)
        {
            var student = context.Students
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
        public IActionResult ViewEnrolledStudents(int courseId)
        {
            var course = context.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return NotFound();

            return View("ViewEnrolledStudents", course);


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

        public IActionResult ShowTeacherCourses(int id)
        {
            var teacher = context.Teachers
                .Include(t => t.Courses)
                .FirstOrDefault(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            var courses = teacher.Courses.ToList();
            return View(courses);
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


        [HttpGet]
        public IActionResult EditTeacher(int id)
        {
            var teacher = context.Teachers.FirstOrDefault(t => t.Id == id);
            if (teacher == null)
                return NotFound();

            return View("EditTeacher", teacher);
        }

        [HttpPost]

        public IActionResult EditTeacher(Teacher updatedTeacher)
        {
            
            if (updatedTeacher != null)
            {
                if (context.Students.Any(s => s.Email == updatedTeacher.Email))
                {
                    ModelState.AddModelError("Email", "The email is already in use by another student.");
                    return View(updatedTeacher);
                }

                var teacher = context.Teachers.FirstOrDefault(t => t.Id == updatedTeacher.Id);
                if (teacher == null)
                    return NotFound();

                teacher.FullName = updatedTeacher.FullName;
                teacher.Email = updatedTeacher.Email;
                teacher.password = updatedTeacher.password;
                teacher.phoneNumber = updatedTeacher.phoneNumber;
                teacher.addres = updatedTeacher.addres;

                context.SaveChanges();
            }

            return RedirectToAction("ShowAllStudents");
        }
        public IActionResult ShowAllCourses()
        {
            List<Course> courses = context.Courses
             .Include(c => c.Enrollments)
             .ThenInclude(e => e.Student)
             .Include(t => t.Teacher)
             .Include(d => d.Department)
             .ToList();

            return View("ShowAllCourses", courses);
        }

        public IActionResult CreateCourse()
        {
            ViewBag.Teachers = context.Teachers
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.FullName
                }).ToList();

            ViewBag.Departments = context.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }).ToList();

            return View("CreateCourse");
        }

        [HttpPost]
        public IActionResult SaveCreateCourse(Course course)
        {


            if (course != null)
            {
                context.Courses.Add(course);
                context.SaveChanges();
                return RedirectToAction("ShowAllCourses");
            }



            ViewBag.Teachers = context.Teachers
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.FullName
                }).ToList();

            ViewBag.Departments = context.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                }).ToList();

            return RedirectToAction("ShowAllCourses", course);
        }
        
        public IActionResult DeleteCourse(int id)
        {
            var course = context.Courses
            .Include(t => t.Teacher).
             Include(d => d.Department)
            .FirstOrDefault(t => t.CourseId == id);

            if (course == null)
                return NotFound();

            context.Courses.Remove(course);
            context.SaveChanges();

            return RedirectToAction("ShowAllCourses");

        }
        // Timetable
        public IActionResult ShowTimetable()
        {
            var grouped = context.Timetables
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
        public IActionResult EditTimetable(int id)
        {
            var timetable = context.Timetables.Find(id);
            if (timetable == null)
            {
                return NotFound();
            }

            ViewBag.Courses = context.Courses
                .Select(c => new SelectListItem { Value = c.CourseId.ToString(), Text = c.Title })
                .ToList();

            ViewBag.Teachers = context.Teachers
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.FullName })
                .ToList();

            return View(timetable);
        }

        [HttpPost]
        public IActionResult EditTimetable(Timetable entry)
        {
            if (entry == null )
            {
                // Populate ViewBag.Courses and ViewBag.Teachers when ModelState is invalid
                ViewBag.Courses = context.Courses
                    .Select(c => new SelectListItem { Value = c.CourseId.ToString(), Text = c.Title })
                    .ToList();

                ViewBag.Teachers = context.Teachers
                    .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.FullName })
                    .ToList();

                return View("EditTimetable", entry);
            }

            // Update the timetable entry
            var existingEntry = context.Timetables.Find(entry.Id);
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

            context.SaveChanges();
            return RedirectToAction("ShowTimetable");
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var entry = context.Timetables.Find(id);
            if (entry == null)
                return NotFound();

            context.Timetables.Remove(entry);
            context.SaveChanges();
            return RedirectToAction("ShowTimetable");
        }

        //exam Timetable 

        public IActionResult ShowExamTimetable()
        {
            var exams = context.Exams.Include(e => e.Course).ToList();

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

       
        [HttpGet("Admin/EditExam/{id}")]
        public IActionResult EditExam(int id)
        {
            var exam = context.Exams.Find(id);
            if (exam == null)
            {
                return NotFound();
            }

            ViewBag.Courses = new SelectList(context.Courses.ToList(), "CourseId", "Title", exam.CourseId);
            return View("EditExam", exam);
        }

        [HttpPost]
        public IActionResult EditExam(Exam exam)
        {
            if (exam == null )
            {
                // Populate ViewBag.Courses to ensure the dropdown is available
                ViewBag.Courses = new SelectList(context.Courses.ToList(), "CourseId", "Title", exam?.CourseId);
                return View("EditExam", exam);
            }

            // Find the existing exam entry
            var existingExam = context.Exams.Find(exam.ExamId);
            if (existingExam == null)
            {
                return NotFound();
            }

            // Update the exam properties
            existingExam.CourseId = exam.CourseId;
            existingExam.ExamDate = exam.ExamDate;
            existingExam.Location = exam.Location;

            // Save changes to the database
            context.SaveChanges();
            return RedirectToAction("ShowExamTimetable");
        }
        [HttpGet]
        public IActionResult CreateExam()
        {
            // Populate ViewBag.Courses for the dropdown
            ViewBag.Courses = new SelectList(context.Courses.ToList(), "CourseId", "Title");
            return View("CreateExam");
        }

        [HttpPost]
        public IActionResult CreateExam(Exam exam)
        {
            if (exam== null)
            {
                // Repopulate ViewBag.Courses if validation fails
                ViewBag.Courses = new SelectList(context.Courses.ToList(), "CourseId", "Title");
                return View("CreateExam", exam);
            }

            // Add the new exam to the database
            context.Exams.Add(exam);
            context.SaveChanges();

            // Redirect to ShowExamTimetable after successful creation
            return RedirectToAction("ShowExamTimetable");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteExam(int id)
        {
            var exam = context.Exams.Find(id);
            if (exam == null)
            {
                return NotFound();
            }

            context.Exams.Remove(exam);
            context.SaveChanges();

            return RedirectToAction("ShowExamTimetable");
        }

    }
}
