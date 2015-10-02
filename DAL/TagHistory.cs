using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    //public enum Reason
    //{
    //    Stolen, Lost, Broken
    //}
    public class TagHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int StudentID { get; set; }
        [IgnorePropertyAttribute]
        public virtual Student Student { get; set; }
        public string DeactivatedTag { get; set; }
        public DateTime DeactivationDate { get; set; }
        public int DeactivationReasonID { get; set; }
        [IgnorePropertyAttribute]
        public virtual DeactivationReason DeactivationReason { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please input Remarks")]
        public string Remarks { get; set; }
    }
}
