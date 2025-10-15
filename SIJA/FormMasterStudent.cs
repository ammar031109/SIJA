using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIJA
{
    public partial class FormMasterStudent : Form
    {
        loginDataContext db = new loginDataContext();
        int selected_id = -1;
        public FormMasterStudent()
        {
            InitializeComponent();
        }

        void showDataCbo()
        {
            var data = new List<string>();
            data.Add("Male");
            data.Add("Female");

            cboGender.DataSource = data;
        }

        void showData()
        {
            dgvData.Columns.Clear();

            var student = db.students.Where(x => x.name.Contains(tbSearch.Text))
                .Select(x => new {
                    x.id,
                    x.name,
                    x.@class,
                    x.gender,
                    x.address,
                    x.phone,
                });

            dgvData.DataSource = student;
        }

        private void FormMasterStudent_Load(object sender, EventArgs e)
        {
          showData();
          showDataCbo();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            showData();
        }

        void clearField()
        {

            tbName.Text = "";
            tbAddress.Text = "";
            tbPhone.Text = "";
            tbClass.Text = "";
            cboGender.Text = "Male";
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (tbName.Text == "" || tbAddress.Text == "" || tbPhone.Text == ""
               || tbClass.Text == "")
            {
                MessageBox.Show("All fields must be filled");
                return;
            }

            var student = new student(); 
            student.name = tbName.Text; 
            student.address = tbAddress.Text;
            student.phone = tbPhone.Text;
            student.@class = tbClass.Text;
            student.gender = cboGender.Text;

            db.students.InsertOnSubmit(student); 
            db.SubmitChanges(); 
            clearField(); 
            showData();
            MessageBox.Show("Data successfully added");
            selected_id = -1;
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selected_id = (int)dgvData.Rows[e.RowIndex].Cells["id"].Value;
                tbName.Text = dgvData.Rows[e.RowIndex].Cells["name"].Value.ToString();
                tbAddress.Text = dgvData.Rows[e.RowIndex].Cells["address"].Value.ToString();
                tbPhone.Text = dgvData.Rows[e.RowIndex].Cells["phone"].Value.ToString();
                tbClass.Text = dgvData.Rows[e.RowIndex].Cells["class"].Value.ToString();
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
                || tbClass.Text == "")
            {
                MessageBox.Show("All fields must be filled"); 
                return;
            }

            var student = db.students.Where(x => x.id == selected_id).FirstOrDefault(); 
            student.name = tbName.Text;
            student.address = tbAddress.Text;
            student.phone = tbPhone.Text;
            student.@class = tbClass.Text;
            student.gender = cboGender.Text;

            db.SubmitChanges(); 
            clearField(); 
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

            var student = db.students.Where(x => x.id == selected_id).FirstOrDefault(); 
            db.students.DeleteOnSubmit(student); 
            db.SubmitChanges(); 
            clearField(); 
            showData(); 
            MessageBox.Show("Data successfully deleted"); 
            selected_id = -1;
        }
    }
}
