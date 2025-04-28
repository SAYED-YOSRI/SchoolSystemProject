using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models
{
    public class Exam
    {
        public int ExamId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime ExamDate { get; set; }
        public string Location { get; set; }
    }

}
