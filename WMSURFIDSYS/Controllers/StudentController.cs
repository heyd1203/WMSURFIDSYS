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
using System.IO;
using CsvHelper;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;

namespace WMSURFIDSYS.Controllers
{
    public class StudentController : Controller
    {
        //[Authorize(Roles = "Admin")]
        // GET: Student
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string option)
        {
            var db = DAL.DbContext.Create();

            int studentId;
            int.TryParse(searchString, out studentId);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
                           
            //IEnumerable<DAL.Student> students = db.Students.All().ToList();
            //students = students.OrderByDescending(s => s.LastName);
            var students = db.Students.All();
            foreach(var student in students)
            {
                student.Course =  db.Courses.Get(student.CourseID);
            }

            //if a user choose the radio button option as Subject  
            if (option == "StudentID") 
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                   students = students.Where(s => s.StudentID.Equals(studentId));
                }
            } 
            if (option == "Name") 
            {  
                 if (!String.IsNullOrEmpty(searchString))
                {
                    students = students.Where(s =>
                    s.LastName.ToUpper().Contains(searchString.ToUpper()) ||
                    s.FirstName.ToUpper().Contains(searchString.ToUpper()));
                 }
            } 
            
           int pageSize = 25;
           int pageNumber = (page ?? 1);
           return View(students.ToPagedList(pageNumber, pageSize));
            //return View(students);
        }

        public ActionResult UploadCSVFile ()
        {
            var db = DAL.DbContext.Create();

            var students = db.Students.All();

            students = students.OrderBy(s => s.EnrollmentDate);

            foreach (var student in students)
            {
                student.Course = db.Courses.Get(student.CourseID);
            }

            return View(students);
        }

        [HttpPost]
        public ActionResult PreviewCSVFile(HttpPostedFileBase uploadFile)
        {
            
            var reader = new StreamReader(uploadFile.InputStream);
            var row = reader.ReadLine(); // read the header

            var list = new List<ImportViewModel>();

            while((row = reader.ReadLine()) != null){
                var splited = row.Split(',');

                list.Add(new ImportViewModel
                {
                    StudentID = Convert.ToInt32(splited[0]),
                    LastName = splited[1],
                    FirstName = splited[2],
                    EnrollmentDate = Convert.ToDateTime(splited[3])
                   
                });
            }
            ViewBag.Message = "Please check the the following data before clicking update.";
            return View(list);
        }

        public ActionResult UpdateEnrollemntDate(List<ImportViewModel> StudentList)
        {
            var db = DAL.DbContext.Create();

            //var studentsToUpdate = new ImportViewModel();
            var students = db.Students.All();

            if (ModelState.IsValid)
            {
                foreach (ImportViewModel updatedStudent in StudentList)
                {
                    var student = students.Where(s => s.StudentID  == updatedStudent.StudentID).First();

                    student.EnrollmentDate = updatedStudent.EnrollmentDate;
                    db.Students.Update(student.Id, student);
                }
                
                return RedirectToAction("Index");
            }

            return View(students);
        }


        public ActionResult Filter(int? page, string option)
        {
             var db = DAL.DbContext.Create();

             IEnumerable<DAL.Student> students = db.Students.All().ToList();

            foreach (var student in students)
            {
                student.Course = db.Courses.Get(student.CourseID);
            }

            //if a user choose the radio button option as Subject  
            if (option == "NoEPC")
            {
                //students = db.Students.All().Where(s => s.EPC is null || s => s.EPC);
                students = db.SearchNoEPC();
            }
            if (option == "WithMessage")
            {
                students = db.Students.All().Where(s => s.Message != null);
            }
            if (option == "NotEnrolled")
            {
                DateTime today = DateTime.Today;
                var validenrollmentdate = db.SelectSemSchoolYear(today);

                var currentEnrollmentStartDate = validenrollmentdate.EnrollmentDateStart;
                var currentSemesterEndSate = validenrollmentdate.SemesterDateEnd;

                //students = db.Students.All().Where(s => s.EnrollmentDate >= currentEnrollmentStartDate && s => s.EnrollmentDate < currentSemesterEndSate);
            }

            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        public class SelectModel
        {
            public int id { get; set; }
            public DateTime EnrollmentDate { get; set; }
            public string Message { get; set; }
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
                studentToUpdate.Message = selectModel.Message;
                //db.EnrollmentDateUpdate(selectModel.id, selectModel.EnrollmentDate);
                db.Students.Update(studentToUpdate.Id, studentToUpdate);
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
                db.TagHistorys.Insert(tagHistory);

                //set EPC to null
                studentToDeactivate.EPC = " ";
                //Update db.Students EPC to null
                db.Students.Update(studentToDeactivate.Id, studentToDeactivate);

                return RedirectToAction("UpdateEPC", new { id = id });
            }

            ViewBag.DeactivationReasonID = new SelectList(db.DeactivationReason.All(), "Id", "Reason", tagHistory.DeactivationReasonID);
            return RedirectToAction("Details", new { id = id });
        }

        public class UpdateEPCModel
        {
            public int Id { get; set; }
            public string EPC { get; set; }
        }

        public ActionResult UpdateEPC(int id)
        {
            var db = DAL.DbContext.Create();

            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEPC(UpdateEPCModel updateEPCModel)
        {
            var db = DAL.DbContext.Create();

            if (updateEPCModel.Id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var studentToUpdate = db.Students.Get(updateEPCModel.Id);

            if (studentToUpdate != null)
            {
                studentToUpdate.Course = db.Courses.Get(studentToUpdate.CourseID);
                studentToUpdate.College = db.Colleges.Get(studentToUpdate.CollegeID);
            }

            if (ModelState.IsValid)
            {
                if (EPCValidate(studentToUpdate))
                {
                    studentToUpdate.EPC = updateEPCModel.EPC;
                    db.Students.Update(studentToUpdate.Id, studentToUpdate);
                    return RedirectToAction("Index");
                }
                
            }

            return View (studentToUpdate);
        }

        private bool EPCValidate(Student student)
        {
            var db = DAL.DbContext.Create();

            var existEPC = db.Students.All().FirstOrDefault(s => s.EPC == student.EPC);

            var isValid = true;

            //validate EPC
            if (existEPC != null)
            {
                isValid = false;
                ModelState.AddModelError("", "Unable to save changes. EPC already exist.");
            }

            return isValid;
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            var db = DAL.DbContext.Create();

            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            var studentViewModel = new StudentDetailsData();

            studentViewModel.Student = db.Students.Get(id);

            if (studentViewModel.Student != null)
            {
                studentViewModel.Student.Course = db.Courses.Get(studentViewModel.Student.CourseID);
                studentViewModel.Student.College = db.Colleges.Get(studentViewModel.Student.CollegeID);

                var tagHistorys = db.SelectStudentTagHistory(studentViewModel.Student.Id);
                
                studentViewModel.TagHistory = tagHistorys;
                
                foreach (var tagHistory in tagHistorys)
                {
                    tagHistory.Student = db.Students.Get(tagHistory.StudentID);
                    tagHistory.DeactivationReason = db.DeactivationReason.Get(tagHistory.DeactivationReasonID);
                }
            }

            //IEnumerable<DAL.TagHistory> tagHistorys = db.TagHistorys.All().ToList();
            //tagHistorys = tagHistorys.OrderByDescending(t => t.DeactivationDate);
            

            if (studentViewModel == null)
            {
                return HttpNotFound();
            }

            return View(studentViewModel);
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