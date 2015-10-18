using DAL;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSURFIDSYS.ViewModel
{
    public class ExportToExcelData
   {
       public List<ExportToExcelData> TapLogList { get; set; }
   }
    
    public class ExportToExcel
    {
        public DateTime DateTimeTap { get; set; }
        public int StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }    
        public string CourseAbbv { get; set; }
       
    }
}