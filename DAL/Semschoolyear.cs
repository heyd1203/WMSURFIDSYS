using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Semschoolyear
    {
       
        public int SemSchoolyearID { get; set; }
        public int? SchoolyearID { get; set; }
        public virtual Schoolyear Schoolyear { get; set; }
        public int SemID { get; set; }
        public virtual Sem Sem { get; set; }
        public DateTime EnrollmentDateStart { get; set; }
        public DateTime SemesterDateEnd { get; set; }
    }
}