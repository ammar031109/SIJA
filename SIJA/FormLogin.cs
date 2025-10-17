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
    public partial class FormLogin : Form
    {
        public static string name;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbName.Text = "Purwanto";
            tbPassword.Text = "pur123";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbName.Text == "" || tbPassword.Text == "")
            {
                MessageBox.Show("All fields must be filled");
                return;

            }

            var db = new DataBaseDataContext();
            var user = db.teachers
                .Where(x => x.name == tbName.Text && x.password == tbPassword.Text).FirstOrDefault();

            if (user != null)
            {
                Helper.id = user.id;
                Helper.password = user.password;
                new FormMain(user.name).Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Your data is not valid!!");
                tbName.Text = "";
                tbPassword.Text = "";
            }
        }
    }
}
