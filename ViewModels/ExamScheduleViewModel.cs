namespace final_project.ViewModels
{
    public class ExamScheduleViewModel
    {
        public DayOfWeek Day { get; set; }
        public List<ExamEntry> Exams { get; set; }

        public class ExamEntry
        {
            public int ExamId { get; set; }
            public string CourseTitle { get; set; }
            public DateTime ExamDate { get; set; }
            public string Location { get; set; }
        }
    }

}
