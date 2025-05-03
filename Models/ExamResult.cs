using System;

namespace final_project.Models

{
    public class ExamResult
    {
        public int ResultId { get; set; }

        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public decimal MarksObtained { get; set; }
        public string Grade { get; set; }
        public string Comments { get; set; }

        public DateTime ResultDate { get; set; } = DateTime.Now;
    }

}
