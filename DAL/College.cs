using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class College
    {
        public int Id { get; set; }

        [Required]
        [StringLength(70)]
        public string CollegeName { get; set; }

        [Required]
        [StringLength(5)]
        public string CollegeAbbv { get; set; }

    }
}
