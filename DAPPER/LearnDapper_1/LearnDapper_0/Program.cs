using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace LearnDapper_0
{
    public class Program
    {
        //https://www.cnblogs.com/flywong/p/9666963.html
        //https://www.w3cschool.cn/dapperorm/dapperorm-toj931f2.html
        //https://medium.com/dapper-net/get-started-with-dapper-net-591592c335aa
        private static string _connection = "InterviewDB";
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings[_connection].ConnectionString;
        static void Main(string[] args)
        {
            try {
                //Student_Class sc = new Student_Class(1, 3);
                //int result = Delete(sc);
                //-------------------------------------
                //List<Student_Class> sclist = new List<Student_Class> {
                //new Student_Class(1, 3), new Student_Class(2, 3), new Student_Class(3, 3), new Student_Class(4, 3), new Student_Class(5, 3), };
                //int result = Delete(sclist);
                //-------------------------------------
                //Student student = new Student();
                //student.studentId = 1;
                //student.studentname = "Tom_Dapper";
                //int result = Update(student);
                //-------------------------------------
                //List<Student> studentlist = new List<Student>() {
                //    new Student { studentId=1, studentname = "Tom"},
                //    new Student { studentId=2, studentname = "Mark_Dapper"},
                //    new Student { studentId=3, studentname = "Jason_Dapper"},
                //    new Student { studentId=4, studentname = "Mary_Dapper"},
                //    new Student { studentId=5, studentname = "Steven_Dapper"}};
                //int result = Update(studentlist);
                //-------------------------------------
                //List<Student> studentlist = new List<Student>();
                //studentlist = Query();
                //-------------------------------------
                //Student student = new Student();
                //student = Query(1);
                //-------------------------------------
                //List<Student> studentList = new List<Student>();
                //int[] ids = { 1, 2, 3, 4, 5 };
                //studentList = QueryIn(ids);
                //-------------------------------------
                //QueryMultiple();
                //-------------------------------------
                //CourseWithTeacher cwt = new CourseWithTeacher();
                //cwt = QueryJoin();
                //-------------------------------------
                //Course cor = new Course();
                //CourseWithTeacher cwt = new CourseWithTeacher();
                //cor.classid = 1;
                //cwt = QueryJoin(cor);
                //-------------------------------------
                //int num = spInsertSingleStudentClass(1, 3);
                //-------------------------------------
                //spInsertMultiStudentClass();

            }
            catch (Exception ex)
            {

            }
        }

        public static int Insert(Student_Class sc)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Execute("insert into student_class (studentid, classid) values (@SId, @CId);", sc);
            }
        }

        public static int Insert(List<Student_Class> sclist)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Execute("insert into student_class (studentid, classid) values (@SId, @CId);", sclist);
            }
        }

        public static int Delete(Student_Class sc)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //var param = new { CId = sc.CId };
                return connection.MO_Execute("delete from student_class where studentid = @SId and classid = @CId", sc, commandTimeout: 180);
            }
        }

        public static int Delete(List<Student_Class> sclist)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.MO_Execute("delete from student_class where studentid = @SId and classid = @CId", sclist, commandTimeout: 180);
            }
        }

        public static int Update(Student student)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.MO_Execute("update Student set studentname=@studentname where studentid=@studentId", student, commandTimeout: 180);
            }
        }

        public static int Update(List<Student> studentlist)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.MO_Execute("update Student set studentname=@studentname where studentid=@studentId", studentlist, commandTimeout: 180);
            }
        }

        public static List<Student> Query()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.MO_Query<Student>("select * from Student").ToList();
            }
        }

        public static Student Query(int studentId)
        {

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.MO_Query<Student>("select * from Student where studentid=@studentId", (new Student(studentId))).SingleOrDefault();
            }
        }

        public static List<Student> QueryIn(int[] ids)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = "select * from Student where studentid in @ids";
                //参数类型是Array的时候，dappper会自动将其转化
                return connection.MO_Query<Student>(sql, new { ids }).ToList();
            }
        }

        public static void QueryMultiple()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = "select * from Student; select * from Class;";
                var multiReader = connection.QueryMultiple(sql);
                var studentList = multiReader.Read<Student>();
                var courseList = multiReader.Read<Course>();
                multiReader.Dispose();
            }
        }

        //One to one strong type relation
        public static CourseWithTeacher QueryJoin()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"select c.classid, c.classname, c.credit, c.enddate, c.startdate, t.teacherid, t.teachername, t.gender
                        from Teacher as t
                        join Class as c
                        on c.teacherid = t.teacherid";
                var result = connection.Query<CourseWithTeacher, Teacher, CourseWithTeacher>(sql,
                (courseWithTeacher, teacher) =>
                {
                    courseWithTeacher.teacher = teacher;
                    return courseWithTeacher;
                },
                splitOn: "teacherid");

                return (CourseWithTeacher)result.FirstOrDefault();
            }
        }

        //One to one strong type relation
        public static CourseWithTeacher QueryJoin(Course course)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var sql = @"select c.classid, c.classname, c.credit, c.enddate, c.startdate, t.teacherid, t.teachername, t.gender
                        from Teacher as t
                        join Class as c
                        on c.teacherid = t.teacherid
                        where c.classid = @classid;";
                var result = connection.Query<CourseWithTeacher, Teacher, CourseWithTeacher>(sql,
                (courseWithTeacher, teacher) =>
                {
                    courseWithTeacher.teacher = teacher;
                    return courseWithTeacher;
                },
                course,
                splitOn: "teacherid");

                return (CourseWithTeacher)result.FirstOrDefault();
            }
        }

        public static int spInsertSingleStudentClass(int studentid, int classid)
        {
            var sql = "createsc";
            int student_classid = 0;

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int affectedRows = connection.Execute(sql,
                    new { student_classid = student_classid, studentid = studentid, classid = classid },
                    commandType: CommandType.StoredProcedure);

                return affectedRows;
            }
        }

        public static int spInsertMultiStudentClass()
        {
            var sql = "createsc";
            int student_classid = 0;

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int affectedRows = connection.Execute(sql,
                    new[] {
                        new {student_classid = student_classid, studentid = 2, classid = 3 },
                        new {student_classid = student_classid, studentid = 3, classid = 3 },
                        new {student_classid = student_classid, studentid = 4, classid = 3 },
                        new {student_classid = student_classid, studentid = 5, classid = 3 }
                    },
                    commandType: CommandType.StoredProcedure);

                return affectedRows;
            }
        }
    }
}
