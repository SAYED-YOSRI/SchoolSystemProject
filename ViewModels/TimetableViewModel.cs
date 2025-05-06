namespace final_project.ViewModels
{
    public class TimetableViewModel
    {
        public string CourseTitle { get; set; }
        public string TeacherName { get; set; }

        public Dictionary<DayOfWeek, List<string>> Schedule { get; set; } = new();
        public Dictionary<DayOfWeek, List<int>> EntryIds { get; set; } = new();
    }
}
