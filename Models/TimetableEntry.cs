using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models
{
    public class TimetableEntry
    {
        public int TimetableEntryId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

}
