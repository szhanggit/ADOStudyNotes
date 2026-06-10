using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnDapper_0
{
    public class Student_Class
    {
        private int _sid = 0;
        private int _cid = 0;

        public Student_Class() { }
        public Student_Class(int StudentId, int ClassId)
        {
            _sid = StudentId;
            _cid = ClassId;
        }
        public int SId { get { return _sid; } set { _sid = value; } }
        public int CId { get { return _cid; } set { _cid = value; } }
    }
}
