﻿using System;
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
    public partial class TeacherManager : KryptonForm
    {
        private const int CS_DropShadow = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = CS_DropShadow;
                return cp;
            }
        }
        private int action; // 0 - add, 1 - edit
        private bool isSelected = false;
        private int currFrom = 1;
        private int pageSize = 10;

        public TeacherManager()
        {
            InitializeComponent();
            LoadStudents();
            LoadComboBoxDepartment();
        }
        private void LoadStudents()
        {
            try
            {
                string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                OracleConnection conn = new OracleConnection(oradb);  // C#
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM ( SELECT a.MAGV \"Teacher ID\", a.MAKHOA \"Department\", a.HO_TEN_GV \"Name\", a.NGAYSINH \"Birth\", a.GIOITINH \"Gender\", a.DIACHI \"Address\", a.LUONG \"Salary\", rownum r__ FROM ( SELECT * FROM SYSTEM.GIAOVIEN ORDER BY MAGV ASC ) a WHERE rownum < ((" + currFrom.ToString() + " * " + pageSize.ToString() + ") + 1 ) ) WHERE r__ >= (((" + currFrom.ToString() + "-1) * " + pageSize.ToString() + ") + 1)";
                cmd.CommandType = CommandType.Text;
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();


                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                dgvStudents.DataSource = dataTable;


                conn.Dispose();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void pbPrev_Click(object sender, EventArgs e)
        {
            if (currFrom > 1)
            {
                currFrom--;
                LoadStudents();
            }
        }

        private void pbNext_Click(object sender, EventArgs e)
        { 
            currFrom++;
            LoadStudents();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        { 
            try
            {
                string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                OracleConnection conn = new OracleConnection(oradb);  // C#
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT a.MAGV \"Teacher ID\", a.MAKHOA \"Department\", a.HO_TEN_GV \"Name\", a.NGAYSINH \"Birth\", a.GIOITINH \"Gender\", a.DIACHI \"Address\", a.LUONG \"Salary\" FROM SYSTEM.GIAOVIEN a WHERE HO_TEN_GV LIKE '%" + txtSearch.Text + "%' OR MAGV LIKE '%" + txtSearch.Text + "%' OR MAKHOA LIKE '%" + txtSearch.Text + "%' OR GIOITINH LIKE '%" + txtSearch.Text + "%' OR DIACHI LIKE '%" + txtSearch.Text + "%'";
                cmd.CommandType = CommandType.Text;
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();


                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                dgvStudents.DataSource = dataTable;

                conn.Dispose();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            currFrom = 1;
            LoadStudents();
            showAction();
            txtSearch.Text = "";
            txtID.Visible = true;
            label10.Visible = true;
            txtID.Text = "";

            txtName.Text = "";
            txtName.Enabled = false;

            txtAddress.Text = "";
            txtAddress.Enabled = false;

            txtPassword.Text = "";
            txtPassword.Enabled = false;

            cbDepartment.Text = "";
            cbDepartment.Enabled = false;

            //cbClass.Text = "";
            //cbClass.Enabled = false;

            txtSalary.Text = "";
            txtSalary.Enabled = false;

            dtpBirth.Enabled = false;

            rbMale.Checked = false;
            rbMale.Enabled = false;
            rbFemale.Checked = false;
            rbFemale.Enabled = false;
        }
        private void showAction()
        {
            pbStudents.Visible = true;
            lbStudents.Visible = true;
            pbEdit.Visible = true;
            lbEdit.Visible = true;
            pbDelete.Visible = true;
            lbDelete.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;
        }

        private void lbStudents_Click(object sender, EventArgs e)
        {
            action = 0;

            pbStudents.Visible = false;
            lbStudents.Visible = false;
            pbEdit.Visible = false;
            lbEdit.Visible = false;
            pbDelete.Visible = false;
            lbDelete.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtID.Visible = false;
            label10.Visible = false;

            txtName.Text = "";
            txtName.Enabled = true;

            txtAddress.Text = "";
            txtAddress.Enabled = true;

            txtPassword.Text = "";
            txtPassword.Enabled = true;

            //cbClass.Text = "";
            //cbClass.Enabled = true;

            cbDepartment.Text = "";
            cbDepartment.Enabled = true;

            txtSalary.Text = "";
            txtSalary.Enabled = true;

            dtpBirth.Enabled = true;

            rbMale.Checked = false;
            rbMale.Enabled = true;
            rbFemale.Checked = false;
            rbFemale.Enabled = true;
        }

        private void dgvStudents_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;
                showAction();

                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                txtID.Text = row.Cells[0].Value.ToString();
                txtName.Text = row.Cells[2].Value.ToString();
                //cbClass.Text = row.Cells[1].Value.ToString();
                txtSalary.Text = row.Cells[6].Value.ToString();
                dtpBirth.Value = DateTime.Parse(row.Cells[3].Value.ToString());
                txtAddress.Text = row.Cells[5].Value.ToString();
                cbDepartment.Text = row.Cells[1].Value.ToString();
                if (row.Cells[4].Value.ToString() == "NAM")
                {
                    rbMale.Checked = true;
                }
                else
                {
                    rbFemale.Checked = true;
                }

            }
        }
        private void LoadComboBoxDepartment()
        {
            try
            {
                string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                OracleConnection conn = new OracleConnection(oradb);  // C#
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT MAKHOA FROM SYSTEM.KHOA";
                cmd.CommandType = CommandType.Text;
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    string sname = reader.GetString(0);
                    cbDepartment.Items.Add(sname.ToString());
                }

                conn.Dispose();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Data";
            for (int i = 1; i < dgvStudents.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dgvStudents.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < dgvStudents.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dgvStudents.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dgvStudents.Rows[i].Cells[j].Value.ToString();
                }
            }
            workbook.SaveAs("Desktop\\Data.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            app.Quit();
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose student to edit!");
                return;
            }
            action = 1;

            pbStudents.Visible = false;
            lbStudents.Visible = false;
            pbEdit.Visible = false;
            lbEdit.Visible = false;
            pbDelete.Visible = false;
            lbDelete.Visible = false;
            pbSave.Visible = true;
            lbSave.Visible = true;

            txtName.Enabled = true;
            txtAddress.Enabled = true;
            txtPassword.Enabled = true;
            cbDepartment.Enabled = true;
            //cbClass.Enabled = true;
            txtSalary.Enabled = true;
            dtpBirth.Enabled = true;
            rbMale.Enabled = true;
            rbFemale.Enabled = true;
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose teacher to delete!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                    OracleConnection conn = new OracleConnection(oradb);  // C#
                    OracleCommand cmd = new OracleCommand("SP_GIAOVIEN_DELETE", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_magv", OracleDbType.Varchar2).Value = txtID.Text;
                    conn.Open();

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    cmd.ExecuteNonQuery();

                    conn.Dispose();
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.Message);
                }
            }
            isSelected = false;

            LoadStudents();
            showAction();

            txtID.Text = "";

            txtName.Text = "";
            txtName.Enabled = false;

            txtAddress.Text = "";
            txtAddress.Enabled = false;

            txtPassword.Text = "";
            txtPassword.Enabled = false;

            cbDepartment.Text = "";
            cbDepartment.Enabled = false;

            //cbClass.Text = "";
            //cbClass.Enabled = false;

            txtSalary.Text = "";
            txtSalary.Enabled = false;

            dtpBirth.Enabled = false;

            rbMale.Checked = false;
            rbMale.Enabled = false;
            rbFemale.Checked = false;
            rbFemale.Enabled = false;
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            if (action == 0)
            {
                try
                {
                    string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                    OracleConnection conn = new OracleConnection(oradb);  // C#
                    OracleCommand cmd = new OracleCommand("SP_GIAOVIEN_ADD", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_makhoa", OracleDbType.Varchar2).Value = cbDepartment.Text;
                    cmd.Parameters.Add("p_hotengv", OracleDbType.Varchar2).Value = txtName.Text;
                    cmd.Parameters.Add("p_gioitinh", OracleDbType.Varchar2).Value = (rbMale.Checked ? "NAM" : "Nữ");
                    cmd.Parameters.Add("p_ngaysinh", OracleDbType.Varchar2).Value = dtpBirth.Value.ToString("dd/MM/yyyy");
                    cmd.Parameters.Add("p_diachi", OracleDbType.Varchar2).Value = txtAddress.Text;
                    cmd.Parameters.Add("p_luong", OracleDbType.Int32).Value = Int32.Parse(txtPassword.Text);
                    cmd.Parameters.Add("p_matkhau", OracleDbType.Varchar2).Value = txtSalary.Text;
                    conn.Open();

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    cmd.ExecuteNonQuery();

                    conn.Dispose();
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.Message);
                }
                MessageBox.Show("Add success");
            }
            else
            {
                if (txtPassword.Text != "")
                {
                    try
                    {
                        string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                        OracleConnection conn = new OracleConnection(oradb);  // C#
                        OracleCommand cmd = new OracleCommand("SP_GIAOVIEN_UDPATE_WITH_PASSWORD", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("p_magv", OracleDbType.Varchar2).Value = txtID.Text;
                        cmd.Parameters.Add("p_makhoa", OracleDbType.Varchar2).Value = cbDepartment.Text;
                        cmd.Parameters.Add("p_hotengv", OracleDbType.Varchar2).Value = txtName.Text;
                        cmd.Parameters.Add("p_gioitinh", OracleDbType.Varchar2).Value = (rbMale.Checked ? "NAM" : "Nữ");
                        cmd.Parameters.Add("p_ngaysinh", OracleDbType.Varchar2).Value = dtpBirth.Value.ToString("dd/MM/yyyy");
                        cmd.Parameters.Add("p_diachi", OracleDbType.Varchar2).Value = txtAddress.Text;
                        cmd.Parameters.Add("p_luong", OracleDbType.Int32).Value = Int32.Parse(txtSalary.Text);
                        cmd.Parameters.Add("p_matkhau", OracleDbType.Varchar2).Value = txtPassword.Text;
                        conn.Open();

                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        cmd.ExecuteNonQuery();

                        conn.Dispose();
                    }
                    catch (Exception es)
                    {
                        MessageBox.Show(es.Message);
                    }
                }
                else
                {
                    try
                    {
                        string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                        OracleConnection conn = new OracleConnection(oradb);  // C#
                        OracleCommand cmd = new OracleCommand("SP_GIAOVIEN_UDPATE", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("p_magv", OracleDbType.Varchar2).Value = txtID.Text;
                        cmd.Parameters.Add("p_makhoa", OracleDbType.Varchar2).Value = cbDepartment.Text;
                        cmd.Parameters.Add("p_hotengv", OracleDbType.Varchar2).Value = txtName.Text;
                        cmd.Parameters.Add("p_gioitinh", OracleDbType.Varchar2).Value = (rbMale.Checked ? "NAM" : "Nữ");
                        cmd.Parameters.Add("p_ngaysinh", OracleDbType.Varchar2).Value = dtpBirth.Value.ToString("dd/MM/yyyy");
                        cmd.Parameters.Add("p_diachi", OracleDbType.Varchar2).Value = txtAddress.Text;
                        cmd.Parameters.Add("p_luong", OracleDbType.Int32).Value = Int32.Parse(txtPassword.Text); 
                        conn.Open();

                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        cmd.ExecuteNonQuery();

                        conn.Dispose();
                    }
                    catch (Exception es)
                    {
                        MessageBox.Show(es.Message);
                    }
                }
                MessageBox.Show("Edit success");
            }

            LoadStudents();
            showAction();
            isSelected = false;

            txtID.Text = "";

            txtName.Text = "";
            txtName.Enabled = false;

            txtAddress.Text = "";
            txtAddress.Enabled = false;

            txtPassword.Text = "";
            txtPassword.Enabled = false;

            cbDepartment.Text = "";
            cbDepartment.Enabled = false;

            //cbClass.Text = "";
            //cbClass.Enabled = false;

            txtSalary.Text = "";
            txtSalary.Enabled = false;

            dtpBirth.Enabled = false;

            rbMale.Checked = false;
            rbMale.Enabled = false;
            rbFemale.Checked = false;
            rbFemale.Enabled = false;
        }
    }
}
