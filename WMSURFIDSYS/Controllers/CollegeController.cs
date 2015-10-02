using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using System.Net;

namespace WMSURFIDSYS.Controllers
{
    public class CollegeController : Controller
    {
        // GET: College
        public ActionResult Index()
        {
            var db = DAL.DbContext.Create();

            IEnumerable<DAL.College> colleges = db.Colleges.All().ToList();

            colleges = colleges.OrderBy(c => c.CollegeName);

            return View(colleges.ToList());
        }

        // GET: Course/Details/5
        public ActionResult Details(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            College colleges = db.Colleges.Get(id);
            if (colleges == null)
            {
                return HttpNotFound();
            }
            return View(colleges);
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
        public ActionResult Create([Bind(Include = "CollegeID,CollegeName,CollegeAbbv")] College college)
        {
            var db = DAL.DbContext.Create();
            if (ModelState.IsValid)
            {
                var collegeId = db.Colleges.Insert(college);

                if (collegeId.HasValue && collegeId.Value > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }

            return View(college);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int id)
        {
            var db = DAL.DbContext.Create();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            College college = db.Colleges.Get(id);
            if (college == null)
            {
                return HttpNotFound();
            }
            return View(college);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(College college, int id)
        {
            var db = DAL.DbContext.Create();
            if (ModelState.IsValid)
            {
                db.Colleges.Update(college.Id, college);
                
                return RedirectToAction("Index");
            }

            return View(college);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int id)
        {
            var db = DAL.DbContext.Create();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            College college = db.Colleges.Get(id);
            if (college == null)
            {
                return HttpNotFound();
            }
            return View(college);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = DAL.DbContext.Create();
            College college = db.Colleges.Get(id);
            bool IsDeleted = db.Colleges.Delete(id);
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
