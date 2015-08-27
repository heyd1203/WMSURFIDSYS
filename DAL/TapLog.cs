using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TapLog
    {
        public int Id { get; set; }
        public int StudentID { get; set; }
        [IgnorePropertyAttribute]//Must be added so that it will not check if the column is present in the database
        public virtual Student Student { get; set; }
        public DateTime DateTimeTap { get; set; } 
    }
}
