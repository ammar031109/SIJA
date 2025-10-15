using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SIJA
{
    public partial class FormMasterTeacher : Form
    {
        loginDataContext db = new loginDataContext();
        int selected_id = -1;
        public FormMasterTeacher()
        {
            InitializeComponent();
        }

        void showDataCbo()
        {
            var data = new List<string>();
            data.Add("MALE");
            data.Add("FEMALE");

            cboGender.DataSource = data;
        }

        void showData()
        {
            dgvData.Columns.Clear();

            var teacher = db.teachers.Where(x => x.name.StartsWith(tbSearch.Text))
               .Select(x => new {
                   x.id,
                   x.name,
                   x.gender,
                   x.address,
                   x.phone,
                   x.subject,
                   x.password
               });

            dgvData.DataSource = teacher;

        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            showData();
        }

        void clearFields()
        {
            tbName.Text = "";
            tbAddress.Text = "";
            tbPhone.Text = "";
            tbSubject.Text = "";
            tbPassword.Text = "";
            cboGender.Text = "Male";
        }

        private void FormMasterTeacher_Load(object sender, EventArgs e)
        {
            showData();
            showDataCbo();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (tbName.Text == "" || tbAddress.Text == "" || tbPhone.Text == ""
                || tbSubject.Text == "" || tbPassword.Text == "")
            {
                MessageBox.Show("All fields must be filled");
                return;
            }

            var teacher = new teacher(); 
            teacher.name = tbName.Text; 
            teacher.address = tbAddress.Text;
            teacher.phone = tbPhone.Text;
            teacher.subject = tbSubject.Text;
            teacher.password = tbPassword.Text;
            teacher.gender = cboGender.Text;

            db.teachers.InsertOnSubmit(teacher);
            db.SubmitChanges(); 
            clearFields(); 
            showData();
            MessageBox.Show("Data successfully added"); 
            selected_id = -1; 
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selected_id = (int)dgvData.Rows[e.RowIndex].Cells["id"].Value;
                tbName.Text = dgvData.Rows[e.RowIndex].Cells["name"].Value.ToString();
                tbAddress.Text = dgvData.Rows[e.RowIndex].Cells["address"].Value.ToString();
                tbPhone.Text = dgvData.Rows[e.RowIndex].Cells["phone"].Value.ToString();
                tbSubject.Text = dgvData.Rows[e.RowIndex].Cells["subject"].Value.ToString();
                tbPassword.Text = dgvData.Rows[e.RowIndex].Cells["password"].Value.ToString();
                cboGender.Text = dgvData.Rows[e.RowIndex].Cells["gender"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selected_id == -1)
            {
                MessageBox.Show("Please select a row to update"); 
                return;
            }

            if (tbName.Text == "" || tbAddress.Text == "" || tbPhone.Text == ""
                || tbSubject.Text == "" || tbPassword.Text == "")
            {
                MessageBox.Show("All fields must be filled");
                return;
            }

            var teacher = db.teachers.Where(x => x.id == selected_id).FirstOrDefault(); 
            teacher.name = tbName.Text; 
            teacher.address = tbAddress.Text;
            teacher.phone = tbPhone.Text;
            teacher.subject = tbSubject.Text;
            teacher.password = tbPassword.Text;
            teacher.gender = cboGender.Text;

            db.SubmitChanges(); 
            clearFields(); 
            showData(); 
            MessageBox.Show("Data successfully updated"); 
            selected_id = -1; 
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selected_id == -1)
            {
                MessageBox.Show("Please select a row to delete"); 
                return;
            }

            var teacher = db.teachers.Where(x => x.id == selected_id).FirstOrDefault(); 
            db.teachers.DeleteOnSubmit(teacher); 
            db.SubmitChanges(); 
            clearFields(); 
            showData();
            MessageBox.Show("Data successfully deleted"); 
            selected_id = -1; 
        }
    }
    
}
