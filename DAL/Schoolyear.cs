using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Schoolyear
    {
        public int SchoolyearID { get; set; }
        public string YearFrom { get; set; }
        public string YearTo { get; set; }
        public string SchoolYearRange
        {
            get
            {
                return YearFrom + " - " + YearTo;
            }
        }
    }
}