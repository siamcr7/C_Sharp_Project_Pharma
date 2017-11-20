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
    public partial class StaffProfile : Form
    {
        private int sID = 0;
        private string picLoc = "";
        public StaffProfile(int id)
        {
            sID = id;
            InitializeComponent();
            init();
        }

        private void init()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {

                var q = from UserLogIN in db.UserLogINs
                        where UserLogIN.Id == sID
                        select UserLogIN;

                foreach (var UserLogIN in q)
                {
                    textBox3.Text = UserLogIN.userName;
                    textBox4.Text = UserLogIN.password;
                    textBox1.Text = UserLogIN.role;
                    textBox5.Text = UserLogIN.DateEmployed.ToString();
                    textBox7.Text = UserLogIN.Salary.ToString();
                    textBox8.Text = UserLogIN.Address;
                    textBox9.Text = UserLogIN.Phone;
                    textBox2.Text = UserLogIN.FullName;
                    textBox10.Text = sID.ToString();
                    
                    try
                    {
                        pictureBox1.Image = new Bitmap(UserLogIN.PicLink);
                        picLoc = UserLogIN.PicLink;
                    }
                    catch
                    {
                        pictureBox1.Image = null;
                    }

                    break;
                }

            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }


        private void StaffProfile_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != textBox6.Text) MessageBox.Show("Check Password Please!");
            else
            {
                try
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    int empId = Int32.Parse(textBox10.Text);

                    UserLogIN q22 = (from UserLogIN in db.UserLogINs
                                     where UserLogIN.Id == empId
                                     select UserLogIN).Single();
                    q22.password = textBox4.Text;
                    q22.Address = textBox8.Text;
                    q22.Phone = textBox9.Text;
                    q22.PicLink = picLoc;
                    q22.FullName = textBox2.Text;
                    db.SubmitChanges();
                    MessageBox.Show("Succesfully updated!\n");
                }

                catch
                {
                    MessageBox.Show("Invalid Input");
                }
            }
        }
    }
}
