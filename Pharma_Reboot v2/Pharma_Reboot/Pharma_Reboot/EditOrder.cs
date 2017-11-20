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
    public partial class EditOrder : Form
    {
        private int adminID = 0;
        public EditOrder(int adminId)
        {
            InitializeComponent();
            loadDomainBoxOrderID();
            loadDomainBoxDrugID();
            loadDomainBoxVendorID();
            this.adminID = adminId;
        }


        private void clearFields()
        {
            domainUpDown2.Text = "";
            textBox3.Text = " ";
            textBox4.Text = " ";
            domainUpDown3.Text = "";
            textBox6.Text = "";
        }

        private void loadDomainBoxOrderID()
        {
            domainUpDown1.Items.Clear();
            DataClasses1DataContext db = new DataClasses1DataContext();
            var q = from n in db.OrderTables
                    where n.OrderStatus == "Ordered" || n.OrderStatus == "Cancelled"
                    select n;
            foreach(var n in q)
            {
                domainUpDown1.Items.Add(n.Id.ToString());
            }
        }

        private void loadDomainBoxDrugID()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var q = from n in db.DrugTables
                    select n;
            foreach (var n in q)
            {
                domainUpDown2.Items.Add(n.Id.ToString());
            }
        }

        private void loadDomainBoxVendorID()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var q = from n in db.VendorTables
                    select n;
            foreach (var n in q)
            {
                domainUpDown3.Items.Add(n.Id.ToString());
            }
        }

        private void add()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            OrderTable dt = new OrderTable();
            try
            {
                // domainUpDown1
                dt.DrugID = Int32.Parse(domainUpDown2.Text);
                dt.DrugName = textBox3.Text;
                dt.QuantityToAdd = Int32.Parse(textBox4.Text);
                dt.AdminID = adminID;
                dt.OrderDate = DateTime.Now;
                //dt.OrderReceiveDate = DateTime.Parse(dateTimePicker2.Text);
                dt.VendorID = Int32.Parse(domainUpDown3.Text);
                dt.OrderStatus = "Ordered";

                db.OrderTables.InsertOnSubmit(dt);
                db.SubmitChanges();

                MessageBox.Show("Succesfully added!\nPlease check in order to receive the order!");
                clearFields();
                loadDomainBoxOrderID();
            }

            catch
            {
                MessageBox.Show("invalid input");
            }
        }

        private void update()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            OrderTable dt = new OrderTable();
            try
            {
                OrderTable q = (from OrderTable in db.OrderTables
                                where OrderTable.Id == Int32.Parse(domainUpDown1.Text)
                                select OrderTable).Single();
                q.DrugID = Int32.Parse(domainUpDown2.Text);
                q.DrugName = textBox3.Text;
                q.QuantityToAdd = Int32.Parse(textBox4.Text);
                //dt.OrderReceiveDate = DateTime.Parse(dateTimePicker2.Text);
                q.VendorID = Int32.Parse(domainUpDown3.Text);

                db.SubmitChanges();
                MessageBox.Show("Succesfully updated!\nPlease check in order to receive the order!");
                clearFields();
                loadDomainBoxOrderID();
            }
            catch
            {
                MessageBox.Show("invalid input\n");
            }
        }

        private void cancel()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            OrderTable dt = new OrderTable();
            try
            {
                var q = from OrderTable in db.OrderTables
                        where OrderTable.Id == Int32.Parse(domainUpDown1.Text)
                        select OrderTable;

                db.OrderTables.DeleteAllOnSubmit(q);
                db.SubmitChanges();
                MessageBox.Show("Succesfully cancelled!\n");
                

                DrugTable q2 = (from DrugTable in db.DrugTables
                               where DrugTable.Id == Int32.Parse(domainUpDown2.Text)
                               select DrugTable).Single();
                q2.Status = "valid";
                db.SubmitChanges();

                clearFields();
                loadDomainBoxOrderID();
            }
            catch
            {
                MessageBox.Show("invalid input\n");
            }
        }
        
        private void radioButtonCheckChange(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                domainUpDown1.Visible = false;
                label1.Visible = false;

                domainUpDown2.Visible = true;
                label2.Visible = true;

                textBox3.Visible = true;
                label3.Visible = true;

                textBox4.Visible = true;
                label4.Visible = true;

                domainUpDown3.Visible = true;
                label8.Visible = true;

                textBox6.Visible = true;
                label10.Visible = true;
            }
            else if (radioButton2.Checked)
            {
                domainUpDown1.Visible = true;
                label1.Visible = true;

                domainUpDown2.Visible = true;
                label2.Visible = true;

                textBox3.Visible = true;
                label3.Visible = true;

                textBox4.Visible = true;
                label4.Visible = true;

                domainUpDown3.Visible = true;
                label8.Visible = true;

                textBox6.Visible = true;
                label10.Visible = true;
            }
            else if (radioButton3.Checked)
            {
                domainUpDown1.Visible = true;
                label1.Visible = true;

                domainUpDown2.Visible = false;
                label2.Visible = false;

                textBox3.Visible = false;
                label3.Visible = false;

                textBox4.Visible = false;
                label4.Visible = false;

                domainUpDown3.Visible = false;
                label8.Visible = false;

                textBox6.Visible = false;
                label10.Visible = false;
            }
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.OrderTables
                        where n.Id == Int32.Parse(domainUpDown1.Text)
                        select n;
                foreach (var n in q)
                {
                    domainUpDown2.Text = n.DrugID.ToString();
                    textBox3.Text = n.DrugName;
                    textBox4.Text = n.QuantityToAdd.ToString();
                    domainUpDown3.Text = n.VendorID.ToString();
                    textBox6.Text = n.VendorTable.Name;
                    break;
                }
            }
            catch
            {
                //MessageBox.Show("invalid input");
                clearFields();
            }
        }

        private void domainUpDown2_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.DrugTables
                        where n.Id == Int32.Parse(domainUpDown2.Text)
                        select n;
                foreach (var n in q)
                {
                    textBox3.Text = n.Name;
                    break;
                }
            }
            catch
            {
                //MessageBox.Show("invalid input");
                textBox3.Text = "";
            }
        }

        private void domainUpDown3_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.VendorTables
                        where n.Id == Int32.Parse(domainUpDown3.Text)
                        select n;
                foreach (var n in q)
                {
                    textBox6.Text = n.Name;
                    break;
                }
            }
            catch
            {
                //MessageBox.Show("invalid input");
                textBox6.Text = "";
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                add();
            }
            else if (radioButton2.Checked)
            {
                update();
            }
            else if (radioButton3.Checked)
            {
                cancel();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            

        }

        private void EditOrder_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    

        
    }
}
