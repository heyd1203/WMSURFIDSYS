using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using System.Net;

namespace WMSURFIDSYS.Controllers
{
    public class SemSchoolYearController : Controller
    {
        // GET: SemSchoolYear
        public ActionResult Index()
        {
            var db = DAL.DbContext.Create();
            //var semSchoolyears = db.SemSchoolYears.Include(s => s.Schoolyear).Include(s => s.Sem);

            IEnumerable<DAL.SemSchoolYear> semschoolyears = db.SemSchoolYears.All().ToList();

            foreach (var semschoolyear in semschoolyears)
            {
                semschoolyear.Schoolyear = db.SchoolYears.Get(semschoolyear.SchoolYearID);
                semschoolyear.Semester = db.Semesters.Get(semschoolyear.SemesterID);
            }
            return View(semschoolyears.ToList());
        }

        // GET: Semschoolyear/Details/5
        public ActionResult Details(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SemSchoolYear semschoolyear = db.SemSchoolYears.Get(id);
            if (semschoolyear == null)
            {
                return HttpNotFound();
            }
            return View(semschoolyear);
        }

        // GET: Semschoolyear/Create
        public ActionResult Create()
        {
            var db = DAL.DbContext.Create();

            var schoolyears = new List<SelectListItem>();
            schoolyears.Add(new SelectListItem()
            {
                Text = "Select a school year",
                Value = "0",
                Selected = true
            });

            schoolyears.AddRange(from sy in db.SchoolYears.All()
                                 select new SelectListItem()
                                 {
                                     Text = sy.YearFrom + " - " + sy.YearTo,
                                     Value = sy.Id.ToString()
                                 });

            var semesters = new List<SelectListItem>();
            semesters.Add(new SelectListItem()
            {
                Text = "Select a semester",
                Value = "0",
                Selected = true
            });

            semesters.AddRange(from s in db.Semesters.All()
                               select new SelectListItem()
                               {
                                   Text = s.SemesterName,
                                   Value = s.Id.ToString()
                               });

            ViewBag.SchoolYearID = schoolyears;
            ViewBag.SemesterID = semesters;
            return View();
        }

        // POST: Semschoolyear/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SemSchoolyearID,SchoolYearID,SemesterID,DateFrom,DateTo")] SemSchoolYear semschoolyear)
        {
            var db = DAL.DbContext.Create();

            if (ModelState.IsValid)
            {
                if (IsValidDate(semschoolyear))
                {
                    db.SemSchoolYears.Insert(semschoolyear);
                    return RedirectToAction("Index");

                }
            }

            ViewBag.SchoolYearID = new SelectList(db.SchoolYears.All(), "Id", "SchoolYearRange", semschoolyear.SchoolYearID);
            ViewBag.SemesterID = new SelectList(db.Semesters.All(), "Id", "SemesterName", semschoolyear.SemesterID);
            return View(semschoolyear);
        }

        private bool IsValidDate(SemSchoolYear semschoolyear)
        {
            var db = DAL.DbContext.Create();

            var date1 = Convert.ToDateTime(semschoolyear.EnrollmentDateStart);
            var date2 = Convert.ToDateTime(semschoolyear.SemesterDateEnd);

            if (date1 <= date2)
            {
                ModelState.AddModelError("", "Unable to save changes. Semester Date End needs to be greater than Enrollment Date Start.");
                return false;
            }
            return true;
        }

        // GET: Semschoolyear/Edit/5
        public ActionResult Edit(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SemSchoolYear semschoolyear = db.SemSchoolYears.Get(id); 
            if (semschoolyear == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchoolyearID = new SelectList(db.SchoolYears.All(), "Id", "SchoolYearRange", semschoolyear.SchoolYearID);
            ViewBag.SemID = new SelectList(db.Semesters.All(), "SemID", "SemName", semschoolyear.SemesterID);
            return View(semschoolyear);
        }

        // POST: Semschoolyear/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SemSchoolyearID,SchoolyearID,SemID,DateFrom,DateTo")] SemSchoolYear semschoolyear)
        {
            var db = DAL.DbContext.Create();

            if (ModelState.IsValid)
            {
                if (IsValidDate(semschoolyear))
                {
                    db.SemSchoolYears.Update(semschoolyear.Id, semschoolyear);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.SchoolyearID = new SelectList(db.SchoolYears.All(), "Id", "SchoolYearRange", semschoolyear.SchoolYearID);
            ViewBag.SemID = new SelectList(db.Semesters.All(), "SemID", "SemName", semschoolyear.SemesterID);
            return View(semschoolyear);
        }

        // GET: Semschoolyear/Delete/5
        public ActionResult Delete(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SemSchoolYear semschoolyear = db.SemSchoolYears.Get(id);
            if (semschoolyear == null)
            {
                return HttpNotFound();
            }
            return View(semschoolyear);
        }

        // POST: Semschoolyear/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = DAL.DbContext.Create();

            bool IsDeleted = db.SemSchoolYears.Delete(id);

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