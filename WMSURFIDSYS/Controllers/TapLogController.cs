using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMSURFIDSYS.Controllers
{
    public class TapLogController : Controller
    {
        // GET: TapLog
        public ActionResult Index(string searchString)
        {
            var db = DAL.DbContext.Create();
            
            IEnumerable<DAL.TapLog> taplogs = db.TapLogs.All().ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                taplogs = taplogs.Where(t => t.StudentID.Equals(searchString));           
            }

            return View(taplogs.ToList());
        }
    }
}