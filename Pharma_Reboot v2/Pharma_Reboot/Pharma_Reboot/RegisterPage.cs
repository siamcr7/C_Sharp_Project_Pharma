using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharma_Reboot
{
    public partial class RegisterPage : Form
    {
        string picLoc = "";
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text != "" && textBox4.Text != "" && textBox1.Text == textBox4.Text)
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    UserLogIN ut = new UserLogIN();

                    var qTemp = from UserLogIN in db.UserLogINs
                                where UserLogIN.userName == textBox3.Text
                                select UserLogIN;
                    bool duplicate = false;
                    foreach (var UserLogIN in qTemp)
                    {
                        duplicate = true;
                        break;
                    }

                    if (!duplicate)
                    {
                        ut.userName = textBox3.Text;
                        ut.password = textBox4.Text;
                        //ut.role = textBox5.Text;

                        if (radioButton1.Checked) ut.role = "admin";
                        else if (radioButton2.Checked) ut.role = "staff";
                        else MessageBox.Show("Role in not Selected!");

                        ut.DateEmployed = DateTime.Now;
                        ut.Salary = 0;
                        ut.Address = textBox8.Text;
                        ut.Phone = textBox9.Text;
                        ut.PicLink = picLoc;
                        ut.FullName = textBox2.Text;
                        ut.Status = "unapproved";

                        db.UserLogINs.InsertOnSubmit(ut);
                        db.SubmitChanges();

                        MessageBox.Show("Successfully Registed! \nWait For Admin Approval!");
                    }
                    else MessageBox.Show("Username already exists!!");


                }
                else if (textBox3.Text == "" || textBox4.Text == "") MessageBox.Show("Must Give A Name and Password!");
                else if (textBox1.Text != textBox4.Text) MessageBox.Show("Password Do not match!");
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int size = -1;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                picLoc = file;
                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                    pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                    //MessageBox.Show(text);
                }
                catch
                {
                    MessageBox.Show("Invalid File Type!");
                }
            }
        }
    }
}
