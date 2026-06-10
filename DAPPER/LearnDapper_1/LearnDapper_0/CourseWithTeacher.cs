using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnDapper_0
{
    public class CourseWithTeacher
    {
        public int classid { get; set; }
        public string classname { get; set; }
        public int credit { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public Teacher teacher { get; set; }
    }
}
