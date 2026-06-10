using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnDapper_0
{
    public class Student
    {
        public Student() { }
        public Student(int SId) { studentId = SId; }
        public int studentId { get; set; }
        public string studentname { get; set; }
        public int gender { get; set; }

    }
}
