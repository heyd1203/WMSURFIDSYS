using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using System.Net;

namespace WMSURFIDSYS.Controllers
{
    public class CourseController: Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            var db = DAL.DbContext.Create();

            IEnumerable<DAL.Course> courses = db.Courses.All().ToList();

            return View(courses.ToList());
        }


        // GET: Course/Details/5
        public ActionResult Details(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Get(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            var db = DAL.DbContext.Create();
            if (ModelState.IsValid)
            {
                var courseId = db.Courses.Insert(course);

                if (courseId.HasValue && courseId.Value > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }

            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int id)
        {
            var db = DAL.DbContext.Create();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Get(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            var db = DAL.DbContext.Create();
            if (ModelState.IsValid)
            {
                db.Courses.Update(course.Id,course);
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int id)
        {
            var db = DAL.DbContext.Create();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Get(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = DAL.DbContext.Create();
            Course course = db.Courses.Get(id);
            bool IsDeleted = db.Courses.Delete(id);
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
