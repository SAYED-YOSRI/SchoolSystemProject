﻿using System.ComponentModel.DataAnnotations.Schema;

namespace final_project.Models
{
    public class Teacher : User
    {

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<Course> Courses { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int TeacherId { get; set; }

    }

}
