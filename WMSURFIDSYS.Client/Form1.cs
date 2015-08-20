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
    public partial class Form1 : Form
    {

        Thread findThread;
        public int m_btnStop = 0;
        public delegate void InvokeDelegate(string str);
        public Form1()
        {
            InitializeComponent();
        }

   
        private void Form1_Load(object sender, EventArgs e)
        {
            USBApi.API_OpenUsb();

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

                   // var existEPC = db.Students.FirstOrDefault(s => s.EPC == epc);
                    var existEPC = db.Students.All().FirstOrDefault(s => s.EPC == epc);

                    if (existEPC != null)
                    {
                        //check valid enrollment date

                        //ValidateEnrollmentDate(existEPC, semschoolyear);
                        //ValidateEnrollmentDate(existEPC, semschoolyear);
                        //display data
                        DisplayStudentInfo(existEPC);
                    }
                    else
                    {
                        DisplayNotValid();
                    }
                }
            }
        }

        //private void ValidateEnrollmentDate(Student student, Semschoolyear semschoolyear)
        //{
        //    DateTime? a = student.EnrollmentDate;
        //    DateTime? b = semschoolyear.EnrollmentDateStart;
        //    DateTime? c = semschoolyear.SemesterDateEnd;

        //    if (a >= b && a <= c)
        //    {
        //        var valid = 1;

        //    }
        //    return valid;
        //}

        private void DisplayStudentInfo(Student student)
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Student>(DisplayStudentInfo), student);
                return;
            }

            //display data
            StudentID.Text = student.StudentID.ToString();

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
            StudentID.Text = "Invalid";
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
    }
}
