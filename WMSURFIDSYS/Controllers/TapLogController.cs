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
            taplogs = taplogs.OrderByDescending(t => t.DateTimeTap);

            int studentId;
            int.TryParse(searchString, out studentId);

            if (!String.IsNullOrEmpty(searchString))
            {
                taplogs = db.SelectStudent(studentId, searchString, searchString);
            }

            foreach (var taplog in taplogs)
            {
                taplog.Student = db.Students.Get(taplog.StudentID);
                taplog.Student.Course = db.Courses.Get(taplog.Student.CourseID);
            }

            return View(taplogs.ToList());
        }

        public ActionResult SearchByDate(DateTime? fromDate, DateTime? toDate)
        {
            var db = DAL.DbContext.Create();

            IEnumerable<DAL.TapLog> taplogs = db.TapLogs.All().ToList();
            taplogs = taplogs.OrderByDescending(t => t.DateTimeTap);

            if (!fromDate.HasValue) fromDate = DateTime.Now.Date;
            if (!toDate.HasValue) toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);
            if (toDate < fromDate) toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;

            var confromDate = Convert.ToDateTime(fromDate);
            var contoDate = Convert.ToDateTime(toDate);

            var addToDate = contoDate.AddDays(1);


            taplogs = db.SelectStudentsDateTap(confromDate, addToDate);
           

            foreach (var taplog in taplogs)
            {
                taplog.Student = db.Students.Get(taplog.StudentID);
                taplog.Student.Course = db.Courses.Get(taplog.Student.CourseID);
            }

            return View(taplogs.ToList());
        }

    }
}