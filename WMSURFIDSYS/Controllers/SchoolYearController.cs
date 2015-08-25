using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using System.Net;
using Dapper;

namespace WMSURFIDSYS.Controllers
{
    public class SchoolYearController : Controller
    {
        // GET: SchoolYear
        public ActionResult Index()
        {
            var db = DAL.DbContext.Create();

            return View(db.SchoolYears.All());
        }

        // GET: Schoolyear/Details/5
        public ActionResult Details(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolYear schoolyear = db.SchoolYears.Get(id);
            if (schoolyear == null)
            {
                return HttpNotFound();
            }
            return View(schoolyear);
        }

        // GET: Schoolyear/Create
        public ActionResult Create()
        {
            var db = DAL.DbContext.Create();
            return View();
        }

        // POST: Schoolyear/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SchoolYear schoolyear)
        {
            var db = DAL.DbContext.Create();

            try
            {
                if (ModelState.IsValid)
                {
                    if (IsValidSchoolYear(schoolyear))
                    {
                        //var existStudent = db.Students.All().FirstOrDefault(s => s.StudentID == student.StudentID);
                        var existSchoolYearFrom = db.SchoolYears.All().FirstOrDefault(s => s.YearFrom == schoolyear.YearFrom);
                        var existSchoolYearTo = db.SchoolYears.All().FirstOrDefault(s => s.YearTo == schoolyear.YearTo);
                        if (existSchoolYearFrom == null && existSchoolYearTo == null)
                        {
                             db.SchoolYears.Insert(schoolyear);
                             return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Dupplicate School Year");
                        }
                    }
                }

            }
            catch (Exception /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(schoolyear);
        }

        private bool IsValidSchoolYear(SchoolYear schoolyear)
        {
            var db = DAL.DbContext.Create();

            //validate yearFrom if value is int
            int yearFrom;
            if (!int.TryParse(schoolyear.YearFrom, out yearFrom))
            {
                ModelState.AddModelError("", "Unable to save changes. Year To needs to be greater than 1 to Year From.");
                return false;
            }
            //validate yearTo if value is int
            int yearTo;
            if (!int.TryParse(schoolyear.YearTo, out yearTo))
            {
                ModelState.AddModelError("", "Unable to save changes. Year To needs to be greater than 1 to Year From.");
                return false;
            }
            //validte if dateTo is greater than dateFrom
            if (yearTo - yearFrom != 1)
            {
                ModelState.AddModelError("", "Unable to save changes. Year To needs to be greater than 1 to Year From.");
                return false;
            }

            return true;
        }



        // GET: Schoolyear/Edit/5
        public ActionResult Edit(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolYear schoolyear = db.SchoolYears.Get(id);
            if (schoolyear == null)
            {
                return HttpNotFound();
            }
            return View(schoolyear);
        }

        // POST: Schoolyear/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SchoolyearID,YearFrom,YearTo")] SchoolYear schoolyear)
        {
            var db = DAL.DbContext.Create();

            if (ModelState.IsValid)
            {
                if (IsValidSchoolYear(schoolyear))
                {
                    db.SchoolYears.Update(schoolyear.Id, schoolyear);
                    return RedirectToAction("Index");

                }
            }
            return View(schoolyear);
        }

        // GET: Schoolyear/Delete/5
        public ActionResult Delete(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolYear schoolyear = db.SchoolYears.Get(id);
            if (schoolyear == null)
            {
                return HttpNotFound();
            }
            return View(schoolyear);
        }

        // POST: Schoolyear/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = DAL.DbContext.Create();

            bool IsDeleted = db.SchoolYears.Delete(id);

            if (IsDeleted)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            var db = DAL.DbContext.Create();

            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}