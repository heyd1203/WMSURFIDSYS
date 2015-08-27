using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMSURFIDSYS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string searchString)
        {
            var db = DAL.DbContext.Create();


            int studentId;
            int.TryParse(searchString, out studentId);

            IEnumerable<DAL.TapLog> taplogs = db.TapLogs.All().ToList();

            if (!String.IsNullOrEmpty(searchString))
            {

                taplogs = db.SelectStudent(studentId, searchString, searchString);

            }

            foreach(var taplog in taplogs)
            {
                taplog.Student = db.Students.Get(taplog.StudentID);
                taplog.Student.Course = db.Courses.Get(taplog.Student.CourseID);
            }

            return View(taplogs.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}