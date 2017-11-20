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
    public partial class EditVendor : Form
    {
        public EditVendor()
        {
            InitializeComponent();
            loadDomainUpDown();
        }

        private void clearFields()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void loadDomainUpDown()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from VendorTable in db.VendorTables
                        select VendorTable;
                foreach (var CataTable in q)
                {
                    domainUpDown1.Items.Add(CataTable.Id.ToString());
                }
            }
            catch
            {

            }
        }

        private void radioButtonCheckChange(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                domainUpDown1.Visible = false;
                label1.Visible = false;               
            }
            else if (radioButton2.Checked)
            {
                domainUpDown1.Visible = true;
                label1.Visible = true;              
            }
        }


        private void add()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            VendorTable dt = new VendorTable();

            try
            {
                dt.Name = (textBox2.Text);
                dt.Address = (textBox3.Text);
                dt.Phone = (textBox4.Text);
                dt.Email = (textBox5.Text);

                db.VendorTables.InsertOnSubmit(dt);
                db.SubmitChanges();

                MessageBox.Show("Succesfully added!\n");
                clearFields();
            }

            catch
            {
                MessageBox.Show("invalid input");
            }
        }

        private void update()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            VendorTable dt = new VendorTable();
            try
            {
                VendorTable q = (from VendorTable in db.VendorTables
                                 where VendorTable.Id == Int32.Parse(domainUpDown1.Text)
                                 select VendorTable).Single();

                if (textBox2.Text != "") q.Name = textBox2.Text;
                if (textBox3.Text != "") q.Address = textBox3.Text;
                if (textBox4.Text != "") q.Phone = textBox4.Text;
                if (textBox5.Text != "") q.Email = textBox5.Text;

                db.SubmitChanges();
                MessageBox.Show("Succesfully updated!\n");
                clearFields();
            }
            catch
            {
                MessageBox.Show("invalid input!\n");
            }
        }


        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from VendorTable in db.VendorTables
                        where VendorTable.Id == Int32.Parse(domainUpDown1.Text)
                        select VendorTable;
                foreach (var qq in q)
                {
                    textBox2.Text = qq.Name;
                    textBox3.Text = qq.Address;
                    textBox4.Text = qq.Phone;
                    textBox5.Text = qq.Email;
                    break;
                }
            }
            catch
            {
                clearFields();
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
        }



        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        { }

        private void button3_Click(object sender, EventArgs e)
        {   }

        private void EditVendor_Load(object sender, EventArgs e)
        {        }



       
    }
}
