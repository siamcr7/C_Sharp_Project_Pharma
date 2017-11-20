using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharma_Reboot
{
    public partial class PaymentPage : Form
    {
        int selectedUserSal = 0;
        public PaymentPage()
        {
            InitializeComponent();
            loadDataGrid1("");
            loadDataGrid2("");
        }

        private void loadDataGrid1(string s)
        {
            if(s == "")
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from SessionTable in db.SessionTables
                        select new
                        {
                            UserID = SessionTable.UserID,
                            UserName = SessionTable.UserLogIN.userName,
                            UserSalary = SessionTable.UserLogIN.Salary,
                            SessionTable.LogInTime,
                            SessionTable.LogOutTime
                        };
                dataGridView1.DataSource = q;
                label6.Text = "Total Work Hour: 0";
            }
            else
            {
                DateTime d1 = DateTime.Parse(dateTimePicker1.Text);
                DateTime d2 = DateTime.Parse(dateTimePicker2.Text);

                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from SessionTable in db.SessionTables
                        where SessionTable.UserLogIN.userName.Contains(s)
                        && SessionTable.LogInTime >= d1 && SessionTable.LogInTime <= d2
                        select new
                        {
                            UserID = SessionTable.UserID,
                            UserName = SessionTable.UserLogIN.userName,
                            UserSalary = SessionTable.UserLogIN.Salary,
                            SessionTable.LogInTime,
                            SessionTable.LogOutTime
                        };
                dataGridView1.DataSource = q;

                DateTime dTot = DateTime.Now;
                DateTime dMinus = DateTime.Now;
                foreach(var SessionTable in q)
                {
                    DateTime dLogIn = SessionTable.LogInTime;
                    DateTime dLogOut = SessionTable.LogOutTime;
                    dTot = dTot.Add(dLogOut.Subtract(dLogIn));
                }
                //dTot = dTot.Subtract(dMinus);
                string diff = dTot.Subtract(dMinus).ToString(@"d\d\ \:h\h\ \:m\m\ \:s\s", System.Globalization.CultureInfo.InvariantCulture);
                label6.Text = "Total Work Hour: "+diff;
            }
            
        }

        private void loadDataGrid2(string s)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            if(s == "")
            {
                var q = from PaymentTable in db.PaymentTables
                        select new
                        {
                            UserID = PaymentTable.UserId,
                            UserName = PaymentTable.UserLogIN.userName,
                            PaymentDate = PaymentTable.DateOfPayment,
                            PaymentTable.PaymentAmount,
                            PaymentTable.ExtraBonus
                        };
                dataGridView2.DataSource = q;
            }
            else
            {
                var q = from PaymentTable in db.PaymentTables
                        where PaymentTable.UserLogIN.userName.Contains(s)
                        select new
                        {
                            UserID = PaymentTable.UserId,
                            UserName = PaymentTable.UserLogIN.userName,
                            PaymentDate = PaymentTable.DateOfPayment,
                            PaymentTable.PaymentAmount,
                            PaymentTable.ExtraBonus
                        };
                dataGridView2.DataSource = q;
            }
            
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            loadDataGrid1(textBox1.Text);
            loadDataGrid2(textBox1.Text);
        }

        private void PaymentPage_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
 
            textBox2.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
            textBox3.Text = "0";
            selectedUserSal = Int32.Parse(dataGridView1.Rows[index].Cells[2].Value.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text == "") textBox3.Text = "0";

                DataClasses1DataContext db = new DataClasses1DataContext();
                PaymentTable dt = new PaymentTable();
                dt.DateOfPayment = DateTime.Today;
                dt.PaymentAmount = selectedUserSal;
                dt.UserId = Int32.Parse(textBox2.Text);
                dt.ExtraBonus = Int32.Parse(textBox3.Text);
                db.PaymentTables.InsertOnSubmit(dt);
                db.SubmitChanges();
                loadDataGrid2("");
                MessageBox.Show("Successfull Payment!");
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
            
        }
    }
}
