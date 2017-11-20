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
    public partial class StaffDetails : Form
    {
        private Form parent;
        private Button[] btArr = new Button[1000];
        // int iBtn = 0;

        public StaffDetails()
        {
            InitializeComponent();
            init();
        }
        public StaffDetails(Form par)
        {
            parent = par;
            InitializeComponent();
            init();
        }

        private void StaffDetails_Load(object sender, EventArgs e)
        {

        }

        private void MyButtonHandler(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;

                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from UserLogIN in db.UserLogINs
                        select UserLogIN;

                foreach (var UserLogIN in q)
                {
                    string sN = UserLogIN.userName;

                    if (sN == btn.Text)
                    {
                        StaffProfile stp = new StaffProfile(UserLogIN.Id);
                        stp.Show();

                        //MessageBox.Show("Button jeta press korso : " + sN );
                        break;
                    }

                }


            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }




        private void init()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from UserLogIN in db.UserLogINs
                        select UserLogIN;

                int iW = 600;
                int iH = 600;

                foreach (var UserLogIN in q)
                {
                    Button b = new Button();
                    b.Name = UserLogIN.userName;
                    b.Text = UserLogIN.userName;
                    b.AutoSize = false;

                    b.Click += new EventHandler(this.MyButtonHandler);

                    string picLink = UserLogIN.PicLink;
                    PictureBox pb = new PictureBox();
                    pb.ImageLocation = picLink;
                    pb.SizeMode = PictureBoxSizeMode.AutoSize;

                    int newWidth = 400;
                    int h = 280;
                    pb.MaximumSize = new Size(newWidth, h);
                    pb.Size = new Size(newWidth, h);


                    FlowLayoutPanel p = new FlowLayoutPanel();
                    p.Controls.Add(pb);
                    p.Controls.Add(b);


                    //iW += 800;
                    //iH += 900;

                    //newWidth = 1000;
                    //h = 550;
                    //p.MaximumSize = new Size(newWidth, h);
                    //p.Size = new Size(newWidth, h);
                    b.Width = 300;
                    flowLayoutPanel1.Controls.Add(pb);
                    flowLayoutPanel1.Controls.Add(b);
                    flowLayoutPanel1.ScrollControlIntoView(pb);
                    b.Location = new System.Drawing.Point(0, 1000);

                }

            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }





    }
}
