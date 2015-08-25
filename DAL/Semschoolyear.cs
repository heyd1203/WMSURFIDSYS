using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SemSchoolYear
    {
       
        public int Id { get; set; }
        public int SchoolYearID { get; set; }
        public virtual SchoolYear Schoolyear { get; set; }
        public int SemesterID { get; set; }
        public virtual Semester Semester { get; set; }
        public DateTime EnrollmentDateStart { get; set; }
        public DateTime SemesterDateEnd { get; set; }
    }
}