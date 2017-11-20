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
    public partial class EditCatagory : Form
    {
        public EditCatagory()
        {
            InitializeComponent();
            loadDomainUpDown();
        }

        private void loadDomainUpDown()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from CatagoryTable in db.CatagoryTables
                        select CatagoryTable;
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

                textBox2.Visible = true;
                label2.Visible = true;
            }
            else if (radioButton2.Checked)
            {
                domainUpDown1.Visible = true;
                label1.Visible = true;

                textBox2.Visible = true;
                label2.Visible = true;
            }
        }

        private void add()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                CatagoryTable dt = new CatagoryTable();
                dt.Name = textBox2.Text;
                db.CatagoryTables.InsertOnSubmit(dt);
                db.SubmitChanges();
                MessageBox.Show("Succesfully added!\n");
                textBox2.Text = "";
            }
            catch
            {
                MessageBox.Show("invalid input");
            }
        }

        private void update()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                CatagoryTable q = (from CatagoryTable in db.CatagoryTables
                                   where CatagoryTable.Id == Int32.Parse(domainUpDown1.Text)
                                   select CatagoryTable).Single();
                q.Name = textBox2.Text;
                db.SubmitChanges();
                MessageBox.Show("Succesfully updated!\n");
                textBox2.Text = "";
            }
            catch
            {
                MessageBox.Show("invalid input");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(domainUpDown1.Text);
            if (radioButton1.Checked)
            {
                add();
            }
            else if (radioButton2.Checked)
            {
                update();
            }
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(domainUpDown1.Text);
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from CatagoryTable in db.CatagoryTables
                        where CatagoryTable.Id == Int32.Parse(domainUpDown1.Text)
                        select CatagoryTable;
                foreach (var nn in q)
                {
                    textBox2.Text = nn.Name;
                    break;
                }
            }
            catch
            {
                textBox2.Text = "";
            }            
        }


        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void CatagoryEdit_Load(object sender, EventArgs e)
        {
        }

     

       
    }
}
