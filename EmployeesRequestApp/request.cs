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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace EmployeesRequestApp
{
    public partial class request : Form
    {
        public request()
        {
            InitializeComponent();
        }
        public static double GetBusinessDays(DateTime startD, DateTime endD)
        {
            double calcBusinessDays =
                1 + ((endD - startD).TotalDays * 5 -
                (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return calcBusinessDays;
        }
        SqlConnection con;
        SqlCommand cmd;
        public double getDayLimit()
        {
            string sorgu = "SELECT DayLimit FROM DayLimits where UserName=@UserName";

            con = new SqlConnection("server=.; Initial Catalog=_QTechDb;Integrated Security=true");
            cmd = new SqlCommand(sorgu, con);

            cmd.Parameters.AddWithValue("@UserName", user);
            con.Open();
            double dayLimit = Convert.ToDouble(cmd.ExecuteScalar());
            con.Close();
            return dayLimit;
        }
        public int getUserId()
        {
            string sorgu = "SELECT Id FROM Users where UserName=@UserName";

            con = new SqlConnection("server=.; Initial Catalog=_QTechDb;Integrated Security=true");
            cmd = new SqlCommand(sorgu, con);

            cmd.Parameters.AddWithValue("@UserName", user);
            con.Open();
            int UserId = Convert.ToInt16(cmd.ExecuteScalar());
            con.Close();
            return UserId;
        }

        public string user = string.Empty;
        private void request_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void requestBtn_Click(object sender, EventArgs e)
        {
            double BusinessDays = GetBusinessDays(dateTimePicker1.Value, dateTimePicker2.Value);
            double UserDayLimit = getDayLimit();
            int userid = getUserId();
            if (BusinessDays <= UserDayLimit)
            {
                cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "update DayLimits set Daylimit=@DayRequest where UserName=@UserName ";
                cmd.Parameters.AddWithValue("@UserName", user);
                cmd.Parameters.AddWithValue("@DayRequest", (UserDayLimit - BusinessDays));
                cmd.ExecuteNonQuery();
                con.Close();

                cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "insert into Request values(@UserId,@UserName,@StartDate,@EndDate,@Workday) ";
                cmd.Parameters.AddWithValue("@UserName", user);
                cmd.Parameters.AddWithValue("@UserId", userid);
                cmd.Parameters.AddWithValue("@StartDate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@EndDate", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@WorkDay", (int)(BusinessDays));
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Job is DONE! Have Fun!");

            }
            else
            {
                MessageBox.Show("Your Limit Is Not Enough!");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                requestBtn.Enabled = false;
                label4.Text = "Check Your Start Day and End Day!";
            }
            else
            {
                requestBtn.Enabled = true;
                double BusinessDays = GetBusinessDays(dateTimePicker1.Value, dateTimePicker2.Value);
                label4.Text = "Total Work Day is : "+ BusinessDays;

            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                requestBtn.Enabled = false;
                label4.Text = "Check Your Start Day and End Day!";
            }
            else
            {
                requestBtn.Enabled = true;
                double BusinessDays = GetBusinessDays(dateTimePicker1.Value, dateTimePicker2.Value);
                label4.Text = "Total Work Day is : " + BusinessDays;
            }
        }
    }
}
