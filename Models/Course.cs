using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<Enrollment> Enrollments { get; set; }
        public List<Exam> Exams { get; set; }
        public List<TimetableEntry> Timetable { get; set; }
    }

}
