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
    public partial class AdminPage : Form
    {
        int uID = 0;
        string picLoc = "";
        Form parent;

        Form[] arrTrackOfOpenedForm = new Form[1000];
        int arrTrackIndex = 0;
        public AdminPage(int id, Form par)
        {
            InitializeComponent();
            init();
            uID = id;
            parent = par;
        }

        // initialize
        void init()
        {
            try
            {
                // datagrid init
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from UserLogIN in db.UserLogINs
                        select UserLogIN;
                dataGridView1.DataSource = q;

                loadWarningNotification();
                loadOrderNotification();
                loadPendingAccountNotification();

                // combo box init
                ComboboxItem item = new ComboboxItem();
                item.Text = "ID";
                comboBox1.Items.Add(item);
                ComboboxItem item2 = new ComboboxItem();
                item2.Text = "Name";
                comboBox1.Items.Add(item2);
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        // warning notification 
        void loadWarningNotification()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                label4.Text = "0 WARNINGS!";
                var q2 = from DrugTable in db.DrugTables
                         where DrugTable.Status == "valid"
                         select DrugTable;
                int quantityLimit = 20;
                DateTime expLimit = System.DateTime.Now.AddDays(60);
                int cntNoti = 0;
                foreach (var DrugTable in q2)
                {
                    int xRem = (int)DrugTable.QuantityBought - (int)DrugTable.QuantitySold;
                    DateTime expDate = (DateTime)DrugTable.ExpiredDate;
                    if (quantityLimit > xRem || expLimit > expDate)
                    {
                        cntNoti++;
                    }
                }
                label4.Text = cntNoti.ToString() + " WARNINGS!";
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        // orderPending noti
        void loadOrderNotification()
        {
            try
            {
                label15.Text = "0 Order Pending";
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from OrderTable in db.OrderTables
                        where OrderTable.OrderStatus == "Pending"
                        select OrderTable;
                int cntPending = 0;
                foreach (var OrderTable in q) cntPending++;
                label15.Text = cntPending.ToString() + " Order Pending";
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        // load pending account list
        void loadPendingAccountNotification()
        {
            try
            {
                label19.Text = "0 Account Pending";
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from UserLogIN in db.UserLogINs
                        where UserLogIN.Status == "unapproved"
                        select UserLogIN;
                int cntPending = 0;
                foreach (var UserLogIN in q) cntPending++;
                label19.Text = cntPending.ToString() + " Account Pending";
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }


        // mouse change
        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.Cursor = Cursors.Hand;
        }
        private void label15_MouseEnter(object sender, EventArgs e)
        {
            label15.Cursor = Cursors.Hand;
        }


        // grid view select for staff profile
        private void dataGridView1DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int index = dataGridView1.CurrentCell.RowIndex;
                //MessageBox.Show(dataGridView1.Rows[index].Cells[0].Value.ToString());
                StaffProfile fm = new StaffProfile(Int32.Parse(dataGridView1.Rows[index].Cells[0].Value.ToString()));
                fm.Show();
                arrTrackOfOpenedForm[arrTrackIndex] = fm;
                arrTrackIndex++;
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        // datagird view cell click
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            textBox10.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
            /// https://www.youtube.com/watch?v=PXZ1OiwU_XM

            //if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            //{

            //    MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            //}
        }

        private void AdminPage_Load(object sender, EventArgs e)
        {

        }

        // search
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                UserLogIN ut = new UserLogIN();

                try
                {
                    if (comboBox1.Text == "ID")
                    {
                        if (textBox1.Text != "")
                        {
                            var q = from UserLogIN in db.UserLogINs
                                    where UserLogIN.Id == Int32.Parse(textBox1.Text)
                                    select UserLogIN;
                            dataGridView1.DataSource = q;
                        }
                        else
                        {
                            var q = from UserLogIN in db.UserLogINs
                                    select UserLogIN;
                            dataGridView1.DataSource = q;
                        }
                    }
                    else
                    {
                        if (textBox1.Text != "")
                        {
                            var q = from UserLogIN in db.UserLogINs
                                    where UserLogIN.userName.Contains(textBox1.Text)
                                    select UserLogIN;
                            dataGridView1.DataSource = q;
                        }
                        else
                        {
                            var q = from UserLogIN in db.UserLogINs
                                    select UserLogIN;
                            dataGridView1.DataSource = q;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid Input");
                }
            }
            else MessageBox.Show("Please mention search by");
        }

        // fetchData
        private void button6_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            UserLogIN ut = new UserLogIN();

            try
            {
                int empId = Int32.Parse(textBox10.Text);

                var q = from UserLogIN in db.UserLogINs
                        where UserLogIN.Id == empId
                        select UserLogIN;

                foreach (var UserLogIN in q)
                {
                    textBox3.Text = UserLogIN.userName;
                    textBox4.Text = UserLogIN.password;
                    //textBox5.Text = UserLogIN.role;

                    if (UserLogIN.role == "admin")
                    {
                        radioButton1.Checked = true;
                    }
                    else if (UserLogIN.role == "staff")
                    {
                        radioButton2.Checked = true;
                    }

                    dateTimePicker1.Text = UserLogIN.DateEmployed.ToString();
                    textBox7.Text = UserLogIN.Salary.ToString();
                    textBox8.Text = UserLogIN.Address;
                    textBox9.Text = UserLogIN.Phone;
                    textBox2.Text = UserLogIN.FullName;
                    comboBox2.SelectedItem = UserLogIN.Status;

                    try
                    {
                        pictureBox1.Image = new Bitmap(UserLogIN.PicLink);
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
                MessageBox.Show("Invalid Input");
            }
        }

        // pic Box
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

        // add
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text != "" && textBox4.Text != "")
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    UserLogIN ut = new UserLogIN();

                    var qTemp = from UserLogIN in db.UserLogINs
                                where UserLogIN.userName == textBox3.Text
                                select UserLogIN;
                    bool duplicate = false;
                    foreach(var UserLogIN in qTemp)
                    {
                        duplicate = true;
                        break;
                    }

                    if(!duplicate)
                    {
                        ut.userName = textBox3.Text;
                        ut.password = textBox4.Text;
                        //ut.role = textBox5.Text;

                        if (radioButton1.Checked) ut.role = "admin";
                        else if (radioButton2.Checked) ut.role = "staff";

                        ut.DateEmployed = DateTime.Parse(dateTimePicker1.Text);
                        if (textBox7.Text != "") ut.Salary = Int32.Parse(textBox7.Text);
                        ut.Address = textBox8.Text;
                        ut.Phone = textBox9.Text;
                        ut.PicLink = picLoc;
                        ut.FullName = textBox2.Text;
                        ut.Status = comboBox2.Text;

                        db.UserLogINs.InsertOnSubmit(ut);
                        db.SubmitChanges();
                        init();

                        MessageBox.Show("Successfully Added!");
                    }
                    else MessageBox.Show("Username already exists!!");                   
                }
                else MessageBox.Show("Must Give A Name and Password!");
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }

        }

        // update
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "")
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                UserLogIN ut = new UserLogIN();

                try
                {
                    int empId = Int32.Parse(textBox10.Text);

                    UserLogIN q22 = (from UserLogIN in db.UserLogINs
                                     where UserLogIN.Id == empId
                                     select UserLogIN).Single();

                    q22.userName = textBox3.Text;
                    q22.password = textBox4.Text;
                    //ut.role = textBox5.Text;

                    if (radioButton1.Checked) q22.role = "admin";
                    else if (radioButton2.Checked) q22.role = "staff";

                    q22.DateEmployed = DateTime.Parse(dateTimePicker1.Text);
                    if (textBox7.Text != "") q22.Salary = Int32.Parse(textBox7.Text);
                    q22.Address = textBox8.Text;
                    q22.Phone = textBox9.Text;
                    q22.PicLink = picLoc;
                    q22.FullName = textBox2.Text;
                    q22.Status = comboBox2.Text;

                    db.SubmitChanges();
                    MessageBox.Show("Succesfully updated!\n");
                    init();
                }

                catch
                {
                    MessageBox.Show("Invalid Input");
                }
            }
            else MessageBox.Show("Must Give name and password");


        }

        // delete/fired
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int empId = Int32.Parse(textBox10.Text);

                if(empId != uID)
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    UserLogIN q = (from UserLogIN in db.UserLogINs
                            where UserLogIN.Id == empId
                            select UserLogIN).Single();

                    q.Status = "invalid";
                    db.SubmitChanges();

                    MessageBox.Show("Succesfully Fired! \n");
                    init();
                }
                else MessageBox.Show("You are currently logged in! \n");

                
            }
            catch
            {
                MessageBox.Show("Invalid Input");
            }


        }


        // when close button is pressed!
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult result = MessageBox.Show("Do you really want to LogOut?", "Warning!", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        DataClasses1DataContext db = new DataClasses1DataContext();
                        UserLogIN q22 = (from UserLogIN in db.UserLogINs
                                         where UserLogIN.Id == uID
                                         select UserLogIN).Single();
                        q22.LastLogoutTime = DateTime.Now;
                        db.SubmitChanges();
                        MessageBox.Show("You are Logged out\nLast LoginTime : " + q22.LastLoginTime.ToString() + "\nLast LogOut Time : " + q22.LastLogoutTime.ToString());

                        SessionTable dt = new SessionTable();
                        dt.UserID = uID;
                        dt.LogInTime = (DateTime)q22.LastLoginTime;
                        dt.LogOutTime = DateTime.Now;
                        db.SessionTables.InsertOnSubmit(dt);
                        db.SubmitChanges();

                        parent.Visible = true;

                        for (int i = 0; i < arrTrackIndex; i++) arrTrackOfOpenedForm[i].Dispose();
                        //this.Dispose();
                        //Application.Exit();
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


        private void AdminPage_Load_1(object sender, EventArgs e)
        {

        }

        //warning notification label click
        private void label4_Click(object sender, EventArgs e)
        {
            WarningPage fm = new WarningPage("Admin", uID);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        // order pendig noti label clk
        private void label15_Click(object sender, EventArgs e)
        {
            OrderPage fm = new OrderPage();
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void mouseEnterIntoPage(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            loadOrderNotification();
            loadWarningNotification();
            loadPendingAccountNotification();
        }

        private void editDrugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditDrug fm = new EditDrug();
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void editOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSales fm = new EditSales();
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void editCatagoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditCatagory fm = new EditCatagory();
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void viewStaffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaffDetails fm = new StaffDetails();
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report_Page fm = new Report_Page();
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void editVendorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditVendor fm = new EditVendor();
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void viewDrugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ViewDrug fm = new ViewDrug(); fm.Show();
            SearchAndView fm = new SearchAndView(viewDrugToolStripMenuItem.Text);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void staffPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PaymentPage fm = new PaymentPage();
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void viewCatagoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //viewCatagoryToolStripMenuItem
            SearchAndView fm = new SearchAndView(viewCatagoryToolStripMenuItem.Text);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void viewOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchAndView fm = new SearchAndView(viewOrderToolStripMenuItem.Text);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void viewSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchAndView fm = new SearchAndView(viewSalesToolStripMenuItem.Text);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void viewSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchAndView fm = new SearchAndView(viewSessionToolStripMenuItem.Text);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void viewPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchAndView fm = new SearchAndView(viewPaymentToolStripMenuItem.Text);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void viewVendorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchAndView fm = new SearchAndView(viewVendorToolStripMenuItem.Text);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void editOrderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EditOrder fm = new EditOrder(uID);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
