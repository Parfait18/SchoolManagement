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
    public partial class StudentsInClassSection : KryptonForm
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
        public StudentsInClassSection()
        {
            InitializeComponent();
            LoadStudents(); 
        }
        private static string GiaoVien = "";
        public StudentsInClassSection(string giaovien)
        {
            GiaoVien = giaovien;
            InitializeComponent();
            LoadStudents();

            label10.Visible = false;
            pbStudents.Visible = false;
            lbStudents.Visible = false;
            pbDelete.Visible = false;
            lbDelete.Visible = false;
        }

        private bool isSelected = false;

        private void LoadStudents()
        {
            if (GiaoVien != "")
            {
                try
                {
                    string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                    OracleConnection conn = new OracleConnection(oradb);  // C#
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT A.MSSV \"Student ID\", A.HO_TEN_SV \"Name\", B.GIUAKI \"Mid term\", B.CUOIKI \"Final term\", B.DTB \"Average\" FROM ( SELECT MSSV, HO_TEN_SV FROM SYSTEM.SINHVIEN ) A, ( SELECT MALOPHP, MSSV, GIUAKI, CUOIKI, DTB FROM SYSTEM.KETQUA WHERE MALOPHP='" + TeacherClassSection.ClassSectionID + "' ) B WHERE A.MSSV = B.MSSV";
                    cmd.CommandType = CommandType.Text;
                    conn.Open();

                    OracleDataReader reader = cmd.ExecuteReader();


                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dgvStudents.DataSource = dataTable;

                    count.Text = dgvStudents.RowCount.ToString() + "/" + ClassSectionManager.limited;

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
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT A.MSSV \"Student ID\", A.HO_TEN_SV \"Name\", B.GIUAKI \"Mid term\", B.CUOIKI \"Final term\", B.DTB \"Average\" FROM ( SELECT MSSV, HO_TEN_SV FROM SYSTEM.SINHVIEN ) A, ( SELECT MALOPHP, MSSV, GIUAKI, CUOIKI, DTB FROM SYSTEM.KETQUA WHERE MALOPHP='" + ClassSectionManager.ClassSectionID + "' ) B WHERE A.MSSV = B.MSSV";
                    cmd.CommandType = CommandType.Text;
                    conn.Open();

                    OracleDataReader reader = cmd.ExecuteReader();


                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dgvStudents.DataSource = dataTable;

                    count.Text = dgvStudents.RowCount.ToString() + "/" + ClassSectionManager.limited;

                    conn.Dispose();
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.Message);
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
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

        private void dgvStudents_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            { 

                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                lbName.Text = row.Cells[1].Value.ToString();
                lbMSSV.Text = row.Cells[0].Value.ToString();
                txtMid.Text = row.Cells[2].Value.ToString();
                txtFinal.Text = row.Cells[3].Value.ToString();
                txtAver.Text = row.Cells[4].Value.ToString();

                isSelected = true;
            }
        }

        private void pbStudents_Click(object sender, EventArgs e)
        {
            StudentClassSectionList studentList = new StudentClassSectionList();
            studentList.ShowDialog();
            LoadStudents();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            txtAver.Text = ((Double.Parse(txtMid.Text) + Double.Parse(txtFinal.Text))/2).ToString();
            if (isSelected)
            {
                try
                {
                    string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                    OracleConnection conn = new OracleConnection(oradb);  // C#
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE SYSTEM.KETQUA SET GIUAKI=" + txtFinal.Text.Replace(',', '.') + ", CUOIKI=" + txtFinal.Text.Replace(',', '.') + ", DTB=" + txtAver.Text.Replace(',', '.') + " WHERE MSSV='" + lbMSSV.Text + "' AND MALOPHP='" + ClassSectionManager.ClassSectionID + "'";
                    cmd.CommandType = CommandType.Text;
                    conn.Open();

                    OracleDataReader reader = cmd.ExecuteReader();

                    MessageBox.Show("Save success");
                    LoadStudents();

                    conn.Dispose();
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.Message);
                }
            }
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (isSelected)
            {
                try
                {
                    string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                    OracleConnection conn = new OracleConnection(oradb);  // C#
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM SYSTEM.KETQUA WHERE MSSV='"+ lbMSSV.Text + "' AND MALOPHP='" + ClassSectionManager.ClassSectionID + "'";
                    cmd.CommandType = CommandType.Text;
                    conn.Open();

                    OracleDataReader reader = cmd.ExecuteReader();

                    MessageBox.Show("Deleted success");
                    LoadStudents();

                    conn.Dispose();
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.Message);
                }
            }
        }
    }
}
