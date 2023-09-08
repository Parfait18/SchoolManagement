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
    public partial class ClassManager : KryptonForm
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

        public static string ClassSectionID;
        public static string SubjectID;
        public static int limited;
        public ClassManager()
        {
            InitializeComponent();
            LoadClasses();
            LoadSubjecs();
            LoadTeachers();
        }
        private void LoadTeachers()
        {
            try
            {
                string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                OracleConnection conn = new OracleConnection(oradb);  // C#
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT MANK FROM SYSTEM.NIENKHOA";
                cmd.CommandType = CommandType.Text;
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    string sname = reader.GetString(0);
                    cbTeacher.Items.Add(sname.ToString());
                }

                conn.Dispose();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }
        private void LoadClasses()
        {
            try
            {
                string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                OracleConnection conn = new OracleConnection(oradb);  // C#
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM ( SELECT a.MALOP \"CLASS ID\", a.MAKHOA \"DEPARTMENT ID\", a.MANK \"SESSION\", a.TENLOP \"CLASS NAME\", a.SISOLOP \"NUMBERS\", rownum r__ FROM ( SELECT * FROM SYSTEM.LOP ORDER BY MALOP ASC ) a WHERE rownum < ((" + currFrom.ToString() + " * " + pageSize.ToString() + ") + 1 ) ) WHERE r__ >= (((" + currFrom.ToString() + "-1) * " + pageSize.ToString() + ") + 1)";
                cmd.CommandType = CommandType.Text;
                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();


                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                dgvClass.DataSource = dataTable;


                conn.Dispose();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }
        private void LoadSubjecs()
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
                    cbSubject.Items.Add(sname.ToString());
                }

                conn.Dispose();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void dgvClass_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                isSelected = true;
                //showAction();

                DataGridViewRow row = dgvClass.Rows[e.RowIndex];
                txtID.Text = row.Cells[0].Value.ToString();
                cbSubject.Text = row.Cells[1].Value.ToString();
                cbTeacher.Text = row.Cells[2].Value.ToString();
                 
                txtSchedule.Text = row.Cells[3].Value.ToString();
                txtNOS.Text = row.Cells[4].Value.ToString();

                ClassSectionID = txtID.Text;
                SubjectID = cbSubject.Text;
                limited = Int32.Parse(txtNOS.Text);
            }
        }

        private void pbStudents_Click(object sender, EventArgs e)
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
            pbDetail.Visible = false;
            lbDetail.Visible = false;

            txtSearch.Text = "";
            txtID.Text = "";
            cbSubject.Text = "";
            cbTeacher.Text = "";
            txtSchedule.Text = "";
            txtNOS.Text = "";

            cbSubject.Enabled = true;
            cbTeacher.Enabled = true;
            txtSchedule.Enabled = true;
            txtNOS.Enabled = true;
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {

            if (!isSelected)
            {
                MessageBox.Show("Please choose class to edit!");
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
            pbDetail.Visible = false;
            lbDetail.Visible = false;

            cbSubject.Enabled = true;
            cbTeacher.Enabled = true; 
            txtSchedule.Enabled = true;
            txtNOS.Enabled = true;
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            if (action == 0)
            {
                try
                {
                    string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                    OracleConnection conn = new OracleConnection(oradb);  // C#
                    OracleCommand cmd = new OracleCommand("SP_LOP_ADD", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_makhoa", OracleDbType.Varchar2).Value = cbSubject.Text;
                    cmd.Parameters.Add("p_mank", OracleDbType.Varchar2).Value = cbTeacher.Text;
                    cmd.Parameters.Add("p_tenlop", OracleDbType.Varchar2).Value = txtSchedule.Text; ; 
                    cmd.Parameters.Add("p_sisolop", OracleDbType.Int32).Value = Int32.Parse(txtNOS.Text);
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
                try
                {
                    string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                    OracleConnection conn = new OracleConnection(oradb);  // C#
                    OracleCommand cmd = new OracleCommand("SP_LOP_UPDATE", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_malop", OracleDbType.Varchar2).Value = txtID.Text;
                    cmd.Parameters.Add("p_makhoa", OracleDbType.Varchar2).Value = cbSubject.Text;
                    cmd.Parameters.Add("p_mank", OracleDbType.Varchar2).Value = cbTeacher.Text;
                    cmd.Parameters.Add("p_tenlop", OracleDbType.Varchar2).Value = txtSchedule.Text; ;
                    cmd.Parameters.Add("p_sisolop", OracleDbType.Int32).Value = Int32.Parse(txtNOS.Text);
                    conn.Open();

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    cmd.ExecuteNonQuery();

                    conn.Dispose();
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.Message);
                }
                MessageBox.Show("Edit success");
            }
            Refesh();
        }

        private void pbReload_Click(object sender, EventArgs e)
        {
            Refesh();
        }
        private void Refesh()
        {
            pbStudents.Visible = true;
            lbStudents.Visible = true;
            pbEdit.Visible = true;
            lbEdit.Visible = true;
            pbDelete.Visible = true;
            lbDelete.Visible = true;
            pbSave.Visible = false;
            lbSave.Visible = false;
            pbDetail.Visible = true;
            lbDetail.Visible = true;

            LoadClasses();
            txtSearch.Text = "";
            txtID.Text = "";
            cbSubject.Text = "";
            cbTeacher.Text = "";
            txtSchedule.Text = "";
            txtNOS.Text = "";

            cbSubject.Enabled = false;
            cbTeacher.Enabled = false; 
            txtSchedule.Enabled = false;
            txtNOS.Enabled = false;
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose class to delete!");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure to delete?", "Confirm", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string oradb = "Data Source=localhost:1521 / ORCL21;User Id=SYSTEM;Password=123;";
                    OracleConnection conn = new OracleConnection(oradb);  // C#
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM SYSTEM.LOP WHERE MALOP='" + txtID.Text + "'";
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr = cmd.ExecuteReader();

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    cmd.ExecuteNonQuery();

                    conn.Dispose();
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.Message);
                }

                Refesh();
            }
        }

        private void pbDetail_Click(object sender, EventArgs e)
        {
            if (!isSelected)
            {
                MessageBox.Show("Please choose class to view!");
                return;
            }
            StudensInClass studentsInClassSection = new StudensInClass();
            studentsInClassSection.ShowDialog();
        }

        private void pbNext_Click(object sender, EventArgs e)
        {

            currFrom++;
            LoadClasses();
        }

        private void pbPrev_Click(object sender, EventArgs e)
        {

            if (currFrom > 1)
            {
                currFrom--;
                LoadClasses();
            }
        }
    }
}
