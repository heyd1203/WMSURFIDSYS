using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMSURFIDSYS.Controllers
{
    public class FileController : Controller
    {
        
        // GET: File
        public ActionResult Index(int id)
        {
            var db = DAL.DbContext.Create();
            var fileToRetrieve = db.Students.Get(id);
            return File(fileToRetrieve.Image, "image/jpeg");
        }
    }
}