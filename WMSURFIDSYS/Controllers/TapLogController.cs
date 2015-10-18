using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMSURFIDSYS.ViewModel;

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


        public ActionResult ExportToExcel(DateTime? fromDate, DateTime? toDate)
        {
            var db = DAL.DbContext.Create();

            GridView gv = new GridView();

            ExportToExcelData exportToExcelData = new ExportToExcelData();

            var confromDate = Convert.ToDateTime(fromDate);
            var contoDate = Convert.ToDateTime(toDate);

            var addToDate = contoDate.AddDays(1);

            var taplogs = db.SelectStudentsDateTap(confromDate, addToDate);
            List<ExportToExcel> list = new List<ExportToExcel>();
            foreach (var taplog in taplogs)
            {
                taplog.Student = db.Students.Get(taplog.StudentID);
                taplog.Student.Course = db.Courses.Get(taplog.Student.CourseID);

                list.Add(new ExportToExcel
                {
                    DateTimeTap = taplog.DateTimeTap,
                    StudentID = taplog.Student.StudentID,
                    LastName = taplog.Student.LastName,
                    FirstName = taplog.Student.FirstName,
                    CourseAbbv = taplog.Student.Course.CourseAbbv
                });
            }

            gv.DataSource = list.ToList();
            gv.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=StudentTapLogList.xls");
            Response.Charset = "";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gv.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
            //return View(list.ToList());
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