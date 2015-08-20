using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DAL
{
    public  class DbContext : Database<DbContext>
    {
        public DbContext() { }

        public static DbContext Create()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            return DbContext.Init(connection, 0);
        }

        public Table<Student> Students { get; set; }
        public Table<Course> Courses { get; set; }
        //public Table<Person> People { get; set; }
        //public Table<File> Files { get; set; }
        public Table<Sem> Sems { get; set; }
        public Table<Schoolyear> Schoolyears { get; set; }
        public Table<Semschoolyear> SemSchoolyears { get; set; }
        public Table<Department> Departments { get; set; }

        private System.Data.IDbConnection CreateConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        public IEnumerable<Student> GetStudents()
        {
            try
            {
                using (var cnn = CreateConnection())
                {
                    cnn.Open();
                    var students = cnn.Query<Student>("select * from students");

                    return students;
                }
            }

            catch (Exception x)
            {
                throw x;
            }
        }

        public Student GetStudentByID(int id)
        {
            Student student = this.Query<Student>("select * from students where id = @id", new { id = id })
                .FirstOrDefault();
           

            return student;
        }

      

        public Student SelectStudent(int id)
        {
            var studentToUpdate = this.Query<Student>("SELECT * FROM students WHERE StudentID = @id", new { id }).SingleOrDefault();

            return studentToUpdate;
        }

        public void EnrollmentDateUpdate(int studentId, DateTime enrollmentDate)
        {
            var sqlQuery =
                "UPDATE Students " +
                "SET EnrollmentDate     = @EnrollmentDate " +
                "WHERE Studentid = @Studentid";

            using (var cnn = CreateConnection())
            {
                cnn.Open();

                cnn.Execute(sqlQuery, new { EnrollmentDate = enrollmentDate, StudentID = studentId });
            }

        }

        public bool UpdateStudent(Student student)
        {
            try
            {
                using (var cnn = CreateConnection())
                {
                    cnn.Open();
                    string sqlQuery = "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, MidName = @MidName, Image = @Image, " +
                    "CourseID = @CourseID, EPC = @EPC,EnrollmentDate = @EnrollmentDate WHERE StudentID=@StudentID";
                    cnn.Execute(sqlQuery, new
                    {
                        student.StudentID,
                        student.FirstName,
                        student.LastName,
                        student.MidName,
                        student.Image,
                        student.CourseID,
                        student.EPC,
                        student.EnrollmentDate
                    });

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

       

        public bool DeleteStudent(int StudentID)
        {
            try
            {
                using (var cnn = CreateConnection())
                {
                    cnn.Open();
                    string sqlQuery = "DELETE FROM Students WHERE StudentID=@StudentID";
                    cnn.Execute(sqlQuery, new { StudentID = StudentID });
                  
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Course> GetCourses()
        {
            try
            {
                using (var cnn = CreateConnection())
                {
                    cnn.Open();
                    var courses = cnn.Query<Course>("select * from courses");

                    return courses;
                }
            }

            catch (Exception x)
            {
                throw x;
            }
        }

        public Course GetCourseByID(int CourseId)
        {
            using (var cnn = CreateConnection())
            {
                cnn.Open();
                string strQuery = string.Format("Select Id, CourseName, CourseAbbv from Courses where " +
                "Id={0}", CourseId);
                var course = cnn.Query<Course>(strQuery).Single<Course>();
                return course;
            }
        }

        public bool UpdateCourse(Course course)
        {
            try
            {
                using (var cnn = CreateConnection())
                {
                    cnn.Open();
                    string sqlQuery = "UPDATE Courses SET CourseName = @CourseName, CourseAbbv = @CourseAbbv WHERE Id=@Id";
                  
                    cnn.Execute(sqlQuery, new
                    {
                        Id = course.Id,
                        course.CourseName,
                        course.CourseAbbv
                    });

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool DeleteCourse(int CourseID)
        {
            try
            {
                using (var cnn = CreateConnection())
                {
                    cnn.Open();
                    string sqlQuery = "DELETE FROM Courses WHERE Id=@Id";
                    cnn.Execute(sqlQuery, new { Id = CourseID });

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }

}
