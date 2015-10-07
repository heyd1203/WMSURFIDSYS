using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using DAL;

namespace WMSURFIDSYS.Client
{
    public partial class WMSURFIDSYS : Form
    {

        Thread findThread;
        public int m_btnStop = 0;
        public delegate void InvokeDelegate(string str);
        public WMSURFIDSYS()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            var db = DAL.DbContext.Create();
            USBApi.API_OpenUsb();

            //display current date and time
            ShowDate.Text = DateTime.Now.ToString("MM dd yyyy");
            ShowTime.Text = DateTime.Now.ToString("hh:mm tt");

            DateTime today = DateTime.Today;
            var validenrollmentdate = db.SelectSemSchoolYear(today);

            if (validenrollmentdate != null)
            {
                validenrollmentdate.Semester = db.Semesters.Get(validenrollmentdate.SemesterID);
                validenrollmentdate.Schoolyear = db.SchoolYears.Get(validenrollmentdate.SchoolYearID);
            }

            ShowSem.Text = validenrollmentdate.Semester.SemesterName.ToString();
            ShowSY.Text = validenrollmentdate.Schoolyear.SchoolYearRange.ToString();

            findThread = new Thread(new ThreadStart(myThread));
            findThread.Start();
        }

        private void myThread()
        {
            var db = DAL.DbContext.Create();

            byte[] data = new byte[100];
            byte[] dataLen = new byte[1];
            int nResult;
            while (true)
            {
                nResult = USBApi.API_InventoryOnce(data, dataLen);
                if (nResult == 0)
                {
                    int i = 0;
                    string onetagInfo = "";
                    for (i = 0; i < dataLen[0]; i++)
                    {
                        String strTmp = string.Format("{0:X2}", data[i]);
                        onetagInfo += strTmp;
                    }

                    string epc = onetagInfo.Substring(4);

                    DateTime today = DateTime.Today;

                    var existEPC = db.Students.All().FirstOrDefault(s => s.EPC == epc);
                    var validenrollmentdate = db.SelectSemSchoolYear(today);

                    if (existEPC != null)
                    {
                        if (ValidateEnrollmentDate(existEPC, validenrollmentdate))
                        {
                            //display data
                            DisplayStudentInfo(existEPC);
                            //record tap logs
                            db.TapLogs.Insert(new TapLog
                            {
                                DateTimeTap = DateTime.Now,
                                StudentID = existEPC.Id
                            });
                        }

                        else 
                        {
                            DisplayStudentInfo(existEPC);
                            DisplayNotEnrolled();
                        }
                                             
                    }
                    else
                    {
                        DisplayNotValid();
                    }
                }
            }
        }

        private bool ValidateEnrollmentDate(Student student, SemSchoolYear semschoolyear)
        {
            var db = DAL.DbContext.Create();

            DateTime? a = student.EnrollmentDate;
            DateTime? b = semschoolyear.EnrollmentDateStart;
            DateTime? c = semschoolyear.SemesterDateEnd;

            if (a >= b && a <= c)
            {
                return true;
            }
            return false;
        }

        private void DisplayStudentInfo(Student student)
        {
            var db = DAL.DbContext.Create();

            //get course
            var students = db.Students.Get(student.Id);
            students.Course = db.Courses.Get(student.CourseID);
            students.College = db.Colleges.Get(student.CollegeID);
            
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Student>(DisplayStudentInfo), student);
                return;
            }

            //display data
            Error.Text = "";
            StudID.Text = student.StudentID.ToString();
            LName.Text = student.LastName.ToString();
            FName.Text = student.FirstName.ToString();
            MName.Text = student.MidName.ToString();
            //Message.Text = student.Message.ToString();
            CAbbv.Text = students.Course.CourseAbbv.ToString();
            //labelCollege.Text = student.College.CollegeName.ToString();
            
            //display image
            image.Image = byteArrayToImage(student.Image);        
        }

        private void DisplayNotValid()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(DisplayNotValid));
                return;
            }

            //display data
            Error.Text = "Student is not registered.";
            StudID.Text = "";
            LName.Text = "Not Registered";
            FName.Text = "";
            MName.Text = "";
            CAbbv.Text = "";
        }

        private void DisplayNotEnrolled()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(DisplayNotEnrolled));
                return;
            }

            //display data
            Error.Text = "Student not currently enrolled.";
        }
        private void image_Click(object sender, EventArgs e)
        {

        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn != null && byteArrayIn.Length > 0)
            {
                var ms = new MemoryStream(byteArrayIn);
                var returnImage = Image.FromStream(ms);


                return returnImage;
            }

            return null;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void StudID_Click(object sender, EventArgs e)
        {

        }

        private void MName_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Message_Click(object sender, EventArgs e)
        {

        }

        private void labelCollege_Click(object sender, EventArgs e)
        {

        }

       
    }
}
