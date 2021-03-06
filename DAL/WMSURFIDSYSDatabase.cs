﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DAL.Properties;

namespace DAL
{
    public class DbContext : Database<DbContext>
    {
        public DbContext() { }

        public static DbContext Create()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            return DbContext.Init(connection, 0);
        }

        public Table<Student> Students { get; set; }
        public Table<Course> Courses { get; set; }
        public Table<College> Colleges { get; set; }
        public Table<Semester> Semesters { get; set; }
        public Table<SchoolYear> SchoolYears { get; set; }
        public Table<SemSchoolYear> SemSchoolYears { get; set; }
        public Table<Department> Departments { get; set; }
        public Table<TapLog> TapLogs { get; set; }
        public Table<TagHistory> TagHistorys { get; set; }
        public Table<DeactivationReason> DeactivationReason { get; set; }

        private System.Data.IDbConnection CreateConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        //public IEnumerable<Student> GetStudents()
        //{
        //    try
        //    {
        //        using (var cnn = CreateConnection())
        //        {
        //            cnn.Open();
        //            var students = cnn.Query<Student>("select * from students");

        //            return students;
        //        }
        //    }

        //    catch (Exception x)
        //    {
        //        throw x;
        //    }
        //}

       
        //public void EnrollmentDateUpdate(int Id, DateTime enrollmentDate)
        //{
        //    var sqlQuery =
        //        "UPDATE Students " +
        //        "SET EnrollmentDate     = @EnrollmentDate " +
        //        "WHERE Id = @Id";

        //    using (var cnn = CreateConnection())
        //    {
        //        cnn.Open();
        //        cnn.Execute(sqlQuery, new { EnrollmentDate = enrollmentDate, Id = Id });
        //    }
        //}

        //public void EPCUpdate(int Id, string epc)
        //{
        //    var sqlQuery =
        //        "UPDATE Students " +
        //        "SET EPC     = @EPC " +
        //        "WHERE Id = @Id";

        //    using (var cnn = CreateConnection())
        //    {
        //        cnn.Open();
        //        cnn.Execute(sqlQuery, new { EPC = epc, Id = Id });
        //    }
        //}
       
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
                        course.CourseName,
                        course.CourseAbbv,
                        course.Id
                    });

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool UpdateSemester(Semester semester)
        {
            try
            {
                using (var cnn = CreateConnection())
                {
                    cnn.Open();
                    string sqlQuery = "UPDATE Semesters SET SemesterName = @SemesterName WHERE Id=@Id";

                    cnn.Execute(sqlQuery, new
                    {
                        semester.SemesterName,
                        semester.Id
                    });

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool DeleteSemester(Semester semester)
        {
            try
            {
                var result = this.Query(Resources.Semester_CheckForDependecies, new { semesterId = semester.Id }).FirstOrDefault();
                return true;
            }

            catch (Exception x)
            {
                return false;
            }

            //return result != null;
        }

        //public bool DeleteStudent(Student student)
        //{
        //    var result = this.Query(Resources.Student_CheckForDependencies, new { studentId = student.StudentID }).FirstOrDefault();

        //    return result != null;
        //}

        public SemSchoolYear SelectSemSchoolYear(DateTime today)
        {
            var selectsemsy = this.Query<SemSchoolYear>("SELECT * FROM semschoolyears WHERE convert(datetime,EnrollmentDateStart)<= @today "+
                                "AND  @today<= convert(datetime,SemesterDateEnd)", new { today }).SingleOrDefault();

            //if (selectsemsy != null)
            //{
            //    this.Query<Semester>("SELECT * FROM semesters WHERE SemesterId=@id", new { SemesterId = Id }).SingleOrDefault();
            //    this.Query<SchoolYear>("SELECT * FROM schoolyears WHERE SchoolYearID=@id", new { selectsemsy.SchoolYearID }).SingleOrDefault();
            //}

            return selectsemsy;
        }

        public IEnumerable<Student> SearchNoEPC()
        {
            try
            {
                using (var cnn = CreateConnection())
                {
                    cnn.Open();
                    var students = cnn.Query<Student>("SELECT [dbo].[Students].* FROM [dbo].[Students] WHERE ([dbo].[Students].[EPC] = '')");
                    return students;
                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        public IList<TapLog> SelectStudent(int studentId, string firstName, string lastName)
        {
            var taplogs = this.Query<TapLog, Student, TapLog>("SELECT [dbo].[TapLogs].*, [dbo].[Students].* FROM [dbo].[TapLogs] INNER JOIN " +
                 "[dbo].[Students] ON [dbo].[TapLogs].[StudentID] = [dbo].[Students].[Id] " +
                 "WHERE ([dbo].[Students].[StudentID] = @StudentID) OR ([dbo].[Students].[LastName] like '%'+ @LastName +'%') OR " +
                 "([dbo].Students.FirstName like '%'+ @FirstName +'%') ORDER BY [dbo].[TapLogs].[DateTimeTap] DESC", (taplog, student) =>
                 {
                     taplog.Student = student;
                     return taplog;
                 },new { studentId, lastName, firstName });
            return taplogs.ToList();

        }

        public IList<TapLog> SelectStudentsDateTap(DateTime fromDate, DateTime toDate)
        {
            var taplogs = this.Query<TapLog, Student, TapLog>("SELECT [dbo].[TapLogs].*, [dbo].[Students].* FROM [dbo].[TapLogs] INNER JOIN " +
                 "[dbo].[Students] ON [dbo].[TapLogs].[StudentID] = [dbo].[Students].[Id] " +
                 "WHERE ([dbo].[TapLogs].[DateTimeTap] >= @fromDate) AND ([dbo].[TapLogs].[DateTimeTap] < @toDate)  " +
                 "ORDER BY [dbo].[TapLogs].[DateTimeTap] DESC", (taplog, student) =>
                 {
                     taplog.Student = student;
                     return taplog;
                 }, new { fromDate, toDate });
            return taplogs.ToList();

        }
        
        public List<TagHistory> SelectStudentTagHistory(int studentId)
        {
            var tagHistorys = this.Query<TagHistory, Student, TagHistory>("SELECT [dbo].[TagHistorys].*, [dbo].[Students].[Id] " +
                 "FROM [dbo].[TagHistorys] INNER JOIN [dbo].[Students] ON [dbo].[TagHistorys].[StudentID] = [dbo].[Students].[Id] " +
                 "WHERE ([dbo].[Students].[Id] = @StudentID) ORDER BY [dbo].[TagHistorys].[DeactivationDate] DESC", (tagHistory, student) =>
                 {
                     tagHistory.Student = student;
                     return tagHistory;
                 }, new { studentId});
            return tagHistorys.ToList();
        }
    }

}
