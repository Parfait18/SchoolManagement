using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Oracle.ManagedDataAccess.Client;

namespace SchoolManagement
{
    public partial class AdminMenu : KryptonForm
    {
        public AdminMenu()
        {
            InitializeComponent();
        }

        private void AdminMenu_Load(object sender, EventArgs e)
        {
            LoadTotals();
        } 

        private void pbLogout_Click(object sender, EventArgs e)
        {
            LogOut();
        }

        private void LogOut()
        {
            Login login = new Login();
            this.Hide();
            login.ShowDialog();
            this.Close();
        }
        private void LoadTotals()
        {
            LoadTotalStudent();
            LoadTotalTeacher();
            LoadTotalClass();
            LoadTotalSubject();
        }
        private void LoadTotalStudent()
        {
            try
            {
                string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                OracleConnection conn = new OracleConnection(oradb);  // C#
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT COUNT(MSSV) FROM SYSTEM.SINHVIEN";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                dr.Read();
                lbToTalStudent.Text = dr.GetString(0);

                conn.Dispose();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void LoadTotalTeacher()
        {
            try
            {
                string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                OracleConnection conn = new OracleConnection(oradb);  // C#
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT COUNT(MAGV) FROM SYSTEM.GIAOVIEN";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                dr.Read();
                lbTotalTeacher.Text = dr.GetString(0);

                conn.Dispose();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void LoadTotalClass()
        {
            try
            {
                string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                OracleConnection conn = new OracleConnection(oradb);  // C#
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT COUNT(MALOP) FROM SYSTEM.LOP";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                dr.Read();
                lbTotalClass.Text = dr.GetString(0);

                conn.Dispose();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void LoadTotalSubject()
        {
            try
            {
                string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                OracleConnection conn = new OracleConnection(oradb);  // C#
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT COUNT(MAMH) FROM SYSTEM.MONHOC";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                dr.Read();
                lbTotalSubject.Text = dr.GetString(0);

                conn.Dispose();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void pbProfile_Click(object sender, EventArgs e)
        {
            AdminProfile myProfile = new AdminProfile();
            myProfile.Show();
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            StudentManager student = new StudentManager(); 
            student.Show(); 
        }

        private void pbTeachers_Click(object sender, EventArgs e)
        {
            TeacherManager teacher = new TeacherManager();
            teacher.Show();
        }

        private void lbTeachers_Click(object sender, EventArgs e)
        {

        }

        private void pbSection_Click(object sender, EventArgs e)
        {
            ClassSectionManager classSectionManager = new ClassSectionManager();
            classSectionManager.Show();
        }

        private void pbClasses_Click(object sender, EventArgs e)
        {
            ClassManager classSectionManager = new ClassManager();
            classSectionManager.Show();
        }

        private void pbSubjects_Click(object sender, EventArgs e)
        {
            SubjectManager subjectManager = new SubjectManager();
            subjectManager.Show();
        }

        private void pbDepartment_Click(object sender, EventArgs e)
        {
            DepartmentManager department = new DepartmentManager();
            department.Show();
        }
    }
}
