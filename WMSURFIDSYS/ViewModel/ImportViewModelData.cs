using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSURFIDSYS.ViewModel
{
    public class ImportViewModelData
    {
        public List<ImportViewModel> StudentList { get; set; }
    }

    public class ImportViewModel
    {
        public int StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        //public List<ImportViewModelData> StudentList { get; set; }
    }
}