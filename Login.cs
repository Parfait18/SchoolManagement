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
    public partial class Login : KryptonForm
    {
        public static string ID;
        public static string TYPE_USER;
        public Login()
        {
            InitializeComponent();
             
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                error.Text = "Invalid ID or Password";
                return;
            }
            try
            {
                string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                OracleConnection conn = new OracleConnection(oradb);  // C#
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT ID_TK, CHUCVU FROM SYSTEM.TAIKHOAN WHERE TENDN='" + txtUsername.Text + "' AND MATKHAU=standard_hash('" + txtPassword.Text + "', 'MD5')";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    ID = dr.GetString(0);
                    TYPE_USER = dr.GetString(1);
                    if (dr.GetString(1) == "Admin")
                    {
                        AdminMenu adminMenu = new AdminMenu();
                        this.Hide();
                        adminMenu.ShowDialog();
                        this.Close();
                    }
                    else if (dr.GetString(1) == "Giáo viên")
                    {
                        TeacherMenu teacherMenu = new TeacherMenu();
                        this.Hide();
                        teacherMenu.ShowDialog();
                        this.Close();
                    }
                    else 
                    {
                        StudentMenu student = new StudentMenu();
                        this.Hide();
                        student.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    error.Text = "Invalid ID or Password";
                }

                conn.Dispose();
            }
            catch (Exception es)
            {
                error.Text = es.Message;
                //MessageBox.Show(es.Message);
            }
        }
    }
}
