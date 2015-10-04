using DAL;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string DeactivatedTag { get; set; }
        public DateTime DeactivationDate { get; set; }
        public int DeactivationReasonID { get; set; }
        [IgnorePropertyAttribute]
        public virtual DeactivationReason DeactivationReason { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please input Remarks")]
        public string Remarks { get; set; }

        //public bool HasEPC {
        //    get {
        //        return !string.IsNullOrEmpty(Student.EPC);           
        //    }
        //}
    }
}