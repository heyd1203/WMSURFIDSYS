using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Student
    {
        public int StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public int CourseID { get; set; }

        [IgnorePropertyAttribute]
        public virtual Course Course { get; set;} 
        public string EPC { get; set; }
        public DateTime? EnrollmentDate { get; set; }

        public byte[] Image { get; set; }

        public int Id { get; set; }
    }
}