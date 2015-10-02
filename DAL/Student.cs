using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Student
    {
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Student Number must be 10 digits")]
        public int StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        [DisplayName("Middle Initial")]
        [StringLength(2, ErrorMessage = "Middle name cannot be longer than 50 characters.")]
        public string MidName { get; set; }
        public int CollegeID { get; set; }

        [IgnorePropertyAttribute]//Must be added so that it will not check if the column is present in the database
        public virtual College College { get; set; }
        public int CourseID { get; set; }

        [IgnorePropertyAttribute]
        public virtual Course Course { get; set;}
        [Required(ErrorMessage = "Please input the EPC")]
        [StringLength(24, MinimumLength = 24, ErrorMessage = "EPC should be 24 characters long.")]
        public string EPC { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? EnrollmentDate { get; set; }

        public byte[] Image { get; set; }

        public int Id { get; set; }
        public string Message { get; set; }
    }
}