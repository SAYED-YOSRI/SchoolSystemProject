using final_project.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace final_project.ViewModels
{
    public class EditTimetableViewModel
    {
        public Timetable TimetableEntry { get; set; }
        public SelectList Courses { get; set; }
        public SelectList Teachers { get; set; }
        public List<DayOfWeek> DaysOfWeek { get; set; }
    }
}