using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSURFIDSYS.ViewModel
{
    public class StudentDetailsData
    {
        //public int Id { get; set; }
        //public int StudentID { get; set; }
        //public string LastName { get; set; }
        //public string FirstName { get; set; }
        //public string MidName { get; set; }
        //public int CollegeName { get; set; }
        //public int CourseAbbv { get; set; }
        //public string EPC { get; set; }
        //public Reason Reason { get; set; }
        //public string Remarks { get; set; }
        public Student Student { get; set; }
        public College College { get; set; }
        public Course Course { get; set; }
        public List<TagHistory> TagHistory { get; set; }
    }
}