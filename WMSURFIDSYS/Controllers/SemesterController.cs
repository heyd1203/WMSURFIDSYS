using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dapper;

namespace WMSURFIDSYS.Controllers
{
    public class SemesterController : Controller
    {
        // GET: Semester
        public ActionResult Index()
        {
            var db = DAL.DbContext.Create();

            return View(db.Semesters.All());
        }

        // GET: Sem/Details/5
        public ActionResult Details(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Semester semester = db.Semesters.Get(id);

            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // GET: Sem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SemesterID,SemesterName")] Semester semester)
        {
            var db = DAL.DbContext.Create();

            if (ModelState.IsValid)
            {
                db.Semesters.Insert(semester);
                return RedirectToAction("Index");
            }

            return View(semester);
        }

        // GET: Sem/Edit/5
        public ActionResult Edit(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Get(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Sem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Semester semester)
        {
            var db = DAL.DbContext.Create();

            if (ModelState.IsValid)
            {
                db.UpdateSemester(semester);
                return RedirectToAction("Index");
            }
            return View(semester);
        }

        // GET: Sem/Delete/5
        public ActionResult Delete(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Get(id);

            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Sem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = DAL.DbContext.Create();

            //Semester semester = db.Semesters.Get(id);
            
            bool IsDeleted = db.Semesters.Delete(id);

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