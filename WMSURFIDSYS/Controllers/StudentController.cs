using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Data.Entity;
using PagedList;
using WMSURFIDSYS.ViewModel;

namespace WMSURFIDSYS.Controllers
{
    public class StudentController : Controller
    {
        //[Authorize(Roles = "Admin")]
        // GET: Student
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var db = DAL.DbContext.Create();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.IDSortParm = sortOrder == "StudentID" ? "id_no" : "StudentID";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //var students = from s in db.Students select s;
                           

            //IEnumerable<DAL.Student> students = db.GetStudents().ToList();
            IEnumerable<DAL.Student> students = db.Students.All().ToList();

            foreach(var student in students)
            {
                student.Course =  db.Courses.Get(student.CourseID);
            }


            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s =>
                s.LastName.ToUpper().Contains(searchString.ToUpper()) ||
                s.FirstName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "id_no":
                    students = students.OrderByDescending(s => s.StudentID);
                    break;
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                //case "course_desc":
                //    students = students.OrderByDescending(s => s.CourseID);
                //    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

           int pageSize = 25;
           int pageNumber = (page ?? 1);
           return View(students.ToPagedList(pageNumber, pageSize));
            //return View(students);
        }

        public class SelectModel
        {
            public int id { get; set; }
            public DateTime EnrollmentDate { get; set; }
        }

        //Display Student Details and Input for Enrollment Date and Message
        public ActionResult Select(int id)
        {
            var db = DAL.DbContext.Create();

            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            //Select Code is found in WMSURFIDSYSDatabase.cs
            //var student = db.GetStudentByID(id);
            var student = db.Students.Get(id);

            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Select(SelectModel selectModel)
        {
            var db = DAL.DbContext.Create();

            if (selectModel.id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentToUpdate = db.Students.Get(selectModel.id);

            if (ModelState.IsValid)
            {
                studentToUpdate.EnrollmentDate = selectModel.EnrollmentDate;
                db.EnrollmentDateUpdate(selectModel.id, selectModel.EnrollmentDate);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public class DeactivateModel
        {
            public int id { get; set; }
            public int DeactivationReasonID { get; set; }
            public virtual ICollection<DeactivationReason> DeactivationReasons { get; set; }
            public string Remarks { get; set; }
        }

        public ActionResult Deactivate(int id)
        {
            var db = DAL.DbContext.Create();

            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentViewModel = new DeactivateTagData();

            studentViewModel.Student = db.Students.Get(id);

            if (studentViewModel.Student != null)
            {
                studentViewModel.Student.Course = db.Courses.Get(studentViewModel.Student.CourseID);
                studentViewModel.Student.College = db.Colleges.Get(studentViewModel.Student.CollegeID);
            }

            if (studentViewModel == null)
            {
                return HttpNotFound();
            }

            var reasons = new List<SelectListItem>();
            reasons.Add(new SelectListItem()
            {
                Text = "Select a reason",
                Value = "0",
                Selected = true
            });

            reasons.AddRange(from r in db.DeactivationReason.All()
                             select new SelectListItem()
                             {
                                 Text = r.Reason,
                                 Value = r.Id.ToString()
                             });



            ViewBag.DeactivationReasonId = reasons;

            //return View();
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(TagHistory tagHistory, int id, DeactivateModel deactivateModel)
        {
            var db = DAL.DbContext.Create();

            var studentToDeactivate = db.Students.Get(id);

            if (studentToDeactivate.Id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
       
            DateTime today = DateTime.Today;
            
            if (ModelState.IsValid)
            {
                tagHistory.StudentID = studentToDeactivate.Id;
                tagHistory.DeactivationDate = today;
                tagHistory.DeactivatedTag = studentToDeactivate.EPC;
                tagHistory.DeactivationReasonID = deactivateModel.DeactivationReasonID;
                //db.EPCDeactivate(deactivateModel.id, deactivateModel.EnrollmentDate);
                db.TagHistorys.Insert(tagHistory);
                studentToDeactivate.EPC = "";
                //Update student.EPC to null
                //db.EnrollmentDateUpdate(selectModel.id, selectModel.EnrollmentDate);
                return RedirectToAction("UpdateEPC", new { id = id });
            }

            ViewBag.DeactivationReasonID = new SelectList(db.DeactivationReason.All(), "Id", "Reason", tagHistory.DeactivationReasonID);
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            var db = DAL.DbContext.Create();

            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Student student = db.Students.Include(s => s.Files).SingleOrDefault(s => s.StudentID == id);

            var student = db.Students.Get(id);

            if (student != null)
            {
                student.Course = db.Courses.Get(student.CourseID);
                student.College = db.Colleges.Get(student.CollegeID);
            }

            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            var db = DAL.DbContext.Create();

            //IEnumerable<SelectListItem> courses = new SelectList(db.GetCourses().Distinct().ToList(), "CourseID", "CourseAbbv");
            //ViewData["courses"] = courses;

            var courses = new List<SelectListItem>();
            var colleges = new List<SelectListItem>();
            courses.Add(new SelectListItem()
            {
                Text = "Select a course",
                Value = "0",
                Selected = true
            });

            colleges.Add(new SelectListItem()
            {
                Text = "Select a college",
                Value = "0",
                Selected = true
            });

            courses.AddRange(from c in db.Courses.All()
                             select new SelectListItem()
                             {
                                 Text = c.CourseAbbv,
                                 Value = c.Id.ToString()
                             });
            colleges.AddRange(from c in db.Colleges.All()
                             select new SelectListItem()
                             {
                                 Text = c.CollegeName,
                                 Value = c.Id.ToString()
                             });

            ViewBag.CourseID = courses;
            ViewBag.CollegeID = colleges;

            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student, HttpPostedFileBase upload)
        {

            var db = DAL.DbContext.Create();

                if (ModelState.IsValid)
                {
                    if (StudentValidate(student))
                    {
                        if (upload != null && upload.ContentLength > 0)
                        {
                            using (var reader = new System.IO.BinaryReader(upload.InputStream))
                            {
                                student.Image = reader.ReadBytes(upload.ContentLength);
                            }

                            var studentId = db.Students.Insert(student);

                            if (studentId.HasValue && studentId.Value > 0)
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return View(student);
                            }
                    }
                }
            }

                //ViewBag.Id = new SelectList(db.GetCourses().Distinct().ToList(), "Id", "CourseAbbv");
                ViewBag.CourseID = new SelectList(db.Courses.All(), "Id", "CourseAbbv", student.CourseID);
                ViewBag.CollegeID = new SelectList(db.Colleges.All(), "Id", "CollegeName", student.CollegeID);

            return View(student);
        }

        private bool StudentValidate(Student student)
        {
            var db = DAL.DbContext.Create();

            var existStudent = db.Students.All().FirstOrDefault(s => s.StudentID == student.StudentID);
            var existEPC = db.Students.All().FirstOrDefault(s => s.EPC == student.EPC);

            var isValid = true;

            //validate student no
            if (existStudent != null)
            {
                isValid = false;
                ModelState.AddModelError("", "Unable to save changes. Student Number already exist.");
            }
            //validate EPC
            if (existEPC != null)
            {
                isValid = false;
                ModelState.AddModelError("", "Unable to save changes. EPC already exist.");
            }

            return isValid;
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            var db = DAL.DbContext.Create();

            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var student = db.Students.Get(id);

           if(student != null)
            {
               student.Course = db.Courses.Get(student.CourseID);
               student.College = db.Colleges.Get(student.CollegeID);
            }

           else
            {
                return HttpNotFound();
            }

            ViewBag.CourseID = new SelectList(db.Courses.All(), "Id", "CourseAbbv", student.CourseID);
            ViewBag.CollegeID = new SelectList(db.Colleges.All(), "Id", "CollegeName", student.CollegeID);
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id, HttpPostedFileBase upload)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var db = DAL.DbContext.Create();

            var studentToUpdate = db.Students.Get(id);

            if (TryUpdateModel(studentToUpdate, "",
                   new string[] { "StudentID","LastName", "FirstName", "MidName","CollegeId", "CourseId", "EPC","EnrollmentDate" }))
            {
                try
                {
                    if (studentToUpdate != null && upload != null && upload.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            studentToUpdate.Image = reader.ReadBytes(upload.ContentLength);
                        }

                        db.Students.Update(studentToUpdate.Id, studentToUpdate);

                        return RedirectToAction("Index");
                    }

                    else
                    {
                        ModelState.AddModelError("", "You have not specified an image file.");

                    }
                }

                catch (Exception /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            ViewBag.CourseId = new SelectList(db.Courses.All(), "Id", "CourseAbbv", studentToUpdate.Id);
            ViewBag.CollegeId = new SelectList(db.Colleges.All(), "Id", "CollegeName", studentToUpdate.Id);
            return View(studentToUpdate);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            var db = DAL.DbContext.Create();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Get(id);

            if (student != null)
            {
                student.Course = db.Courses.Get(student.CourseID);
                student.College = db.Colleges.Get(student.CollegeID);
            }
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        [ActionName ("Delete")]
        public ActionResult DeletePost(int id)
        {
            var db = DAL.DbContext.Create();

            try
            {
                Student student = db.Students.Get(id);
                bool IsDeleted = db.Students.Delete(id);
                if (IsDeleted)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(student);
                }
            }
            catch (Exception dex )
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            //return RedirectToAction("Index");
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