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
using System.Globalization;

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

            var filter = new MessageFilter();
            filter.CodeReceive += filter_CodeReceive;
            Application.AddMessageFilter(filter);
            this.FormClosed += (s, e) => Application.RemoveMessageFilter(filter);
        }

        void filter_CodeReceive(object sender, EventArgs e)
        {
            var epc = ((MessageFilter)sender).Message;

            FindEPC(epc);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;

            var db = DAL.DbContext.Create();
            //display current date and time
            ShowDate.Text = DateTime.Now.ToString("dd MMM yyyy");
            ShowTime.Text = DateTime.Now.ToString("HH:mm tt");

            DateTime today = DateTime.Today;
            var validenrollmentdate = db.SelectSemSchoolYear(today);

            if (validenrollmentdate != null)
            {
                validenrollmentdate.Semester = db.Semesters.Get(validenrollmentdate.SemesterID);
                validenrollmentdate.Schoolyear = db.SchoolYears.Get(validenrollmentdate.SchoolYearID);
            }

            ShowSem.Text = validenrollmentdate.Semester.SemesterName.ToString();
            ShowSY.Text = validenrollmentdate.Schoolyear.SchoolYearRange.ToString();

            try
            {
                USBApi.API_OpenUsb();
                findThread = new Thread(new ThreadStart(myThread));
                findThread.Start();
            }
            catch
            {
               
            }

        }

        private void FindEPC(string epc)
        {
            var db = DAL.DbContext.Create();


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

                    FindEPC(epc);
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

            //get colleges and courses
            var students = db.Students.Get(student.Id);
            students.College = db.Colleges.Get(student.CollegeID);
            students.Course = db.Courses.Get(student.CourseID);


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
            CollegeAbbv.Text = students.College.CollegeName.ToString();
            CourseAbbv.Text = students.Course.CourseAbbv.ToString();

            //display image
            pictureBox1.Image = byteArrayToImage(student.Image);

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
            StudID.Text = "Not registered.";
            LName.Text = "Not registered.";
            FName.Text = "Not registered.";
            MName.Text = "Not registered.";
            CollegeAbbv.Text = "Not registered.";
            CourseAbbv.Text = "Not registered.";

            //Image image = Image.FromFile(@"Images\not_registered.png", true);
            //pictureBox1.Image = image;
            //pictureBox1.Height = image.Height;
            //pictureBox1.Width = image.Width;

        }

        private void DisplayNotEnrolled()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(DisplayNotEnrolled));
                return;
            }

            //display error message
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

        private void ShowSem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ShowTime_Click(object sender, EventArgs e)
        {

        }

        private void Error_Click(object sender, EventArgs e)
        {
           
        }
    }
}
