using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace final_project.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public Grade? Grade { get; set; }

        public ICollection<ExamResult> ExamResults { get; set; }
    }

}
