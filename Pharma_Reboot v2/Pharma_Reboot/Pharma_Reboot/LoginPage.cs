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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label3.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                UserLogIN ut = new UserLogIN();

                string uN = textBox1.Text;
                string pass = textBox2.Text;

                var q = from UserLogIN in db.UserLogINs
                        where UserLogIN.userName == uN && UserLogIN.password == pass && UserLogIN.Status == "valid"
                        select UserLogIN;
                bool logIn = false;

                foreach (var UserLogIN in q)
                {
                    string qN = UserLogIN.userName;
                    string qP = UserLogIN.password;

                    if (qN == uN && pass == qP)
                    {
                        if (UserLogIN.role == "admin")
                        {
                            MessageBox.Show("Successfully Loggedin as Admin");

                            AdminPage fm = new AdminPage(UserLogIN.Id,this);
                            this.Visible = false;
                            fm.Show();

                            UserLogIN q22 = (from UserLogINn in db.UserLogINs
                                             where UserLogINn.Id == UserLogIN.Id
                                             select UserLogINn).Single();
                            q22.LastLoginTime = DateTime.Now;
                            db.SubmitChanges();

                            label3.Text = "You have been logged out!";
                            textBox1.Text = "";
                            textBox2.Text = "";
                            logIn = true;
                            break;
                        }
                        else if (UserLogIN.role == "staff")
                        {
                            MessageBox.Show("Successfully Loggedin as Staff");

                            SalesInfo fm = new SalesInfo(UserLogIN.Id, UserLogIN.userName,this);
                            this.Visible = false;
                            fm.Show();
                            UserLogIN q22 = (from UserLogINn in db.UserLogINs
                                             where UserLogINn.Id == UserLogIN.Id
                                             select UserLogINn).Single();
                            q22.LastLoginTime = DateTime.Now;
                            db.SubmitChanges();

                            label3.Text = "You have been logged out!";
                            logIn = true;
                            break;
                        }

                    }
                }

                if (!logIn) label3.Text = "Username and password do not match";

            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // when close button is pressed!
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult result = MessageBox.Show("Do you really want to exit the Application?", "Warning!", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegisterPage fm = new RegisterPage();
            fm.Show();
        }
    }
}
