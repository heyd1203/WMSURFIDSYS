using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(70)]
        public string CourseName { get; set; }
        
        [Required]
        [StringLength(10)]
        public string CourseAbbv { get; set; }
    }
}