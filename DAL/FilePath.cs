using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FilePath
    {
        public int FilePathId { get; set; }
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public int PersonID { get; set; }
        public virtual Person Person { get; set; }
    }
}