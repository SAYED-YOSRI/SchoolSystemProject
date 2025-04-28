namespace final_project.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Course> Courses { get; set; }
    }
}
