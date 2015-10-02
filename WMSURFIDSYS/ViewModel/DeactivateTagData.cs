using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSURFIDSYS.ViewModel
{
    public class DeactivateTagData
    {
        public Student Student { get; set; }
        public College College { get; set; }
        public Course Course { get; set; }
        public TagHistory TagHistory { get; set; }

        public bool HasEPC {
            get {
                return !string.IsNullOrEmpty(Student.EPC);           
            }
        }
    }
}