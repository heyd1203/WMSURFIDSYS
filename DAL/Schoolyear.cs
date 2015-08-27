using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SchoolYear
    {
        public int Id { get; set; }
        public string YearFrom { get; set; }
        public string YearTo { get; set; }
        [IgnorePropertyAttribute]
        public string SchoolYearRange
        {
            get
            {
                return YearFrom + " - " + YearTo;
            }
        }
    }
}