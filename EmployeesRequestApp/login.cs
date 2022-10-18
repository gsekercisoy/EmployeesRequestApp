using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeesRequestApp
{
    public partial class LOGIN : Form
    {
        public LOGIN()
        {
            InitializeComponent();
        }
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;
        private void button1_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM Users where UserName=@userName AND UserPassword=@pass";

            con = new SqlConnection("server=.; Initial Catalog=_QTechDb;Integrated Security=true");
            cmd = new SqlCommand(sorgu, con);

            cmd.Parameters.AddWithValue("@userName", idTbx.Text);
            cmd.Parameters.AddWithValue("@pass", passwordTbx.Text);

            con.Open();
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                this.Hide();
                request f2 = new request();
                f2.user = idTbx.Text;
                f2.Show();

            }
            else
            {
                MessageBox.Show("Check Your Id and Password and Try Again!");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LOGIN_Load(object sender, EventArgs e)
        {
            passwordTbx.PasswordChar = '*';
        }
    }
}
