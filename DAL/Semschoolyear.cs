using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class SemSchoolYear
    {
       
        public int Id { get; set; }
        public int SchoolYearID { get; set; }
        [IgnorePropertyAttribute]
        public virtual SchoolYear Schoolyear { get; set; }
        public int SemesterID { get; set; }
        [IgnorePropertyAttribute]
        public virtual Semester Semester { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EnrollmentDateStart { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime SemesterDateEnd { get; set; }
       
    }
}