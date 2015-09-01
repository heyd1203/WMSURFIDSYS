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
        public ActionResult Index(string searchString, string sortOrder)
        {
            var db = DAL.DbContext.Create();

            ViewBag.DateTimeTapSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            IEnumerable<DAL.TapLog> taplogs = db.TapLogs.All().ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                taplogs = taplogs.Where(t => t.StudentID.Equals(searchString));           
            }

            switch (sortOrder)
            {
                case "id_no":
                    taplogs = taplogs.OrderByDescending(t => t.DateTimeTap.Date.TimeOfDay);
                    break;
                default:
                    taplogs = taplogs.OrderByDescending(t => t.DateTimeTap.Date.TimeOfDay);
                    break;
            }

            return View(taplogs.ToList());
        }
    }
}