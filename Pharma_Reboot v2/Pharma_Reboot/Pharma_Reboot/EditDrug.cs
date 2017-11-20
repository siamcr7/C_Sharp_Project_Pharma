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
    public partial class EditDrug : Form
    {
        public EditDrug()
        {
            InitializeComponent();
            loadCatagoryComboBox1();
            loadDomainUpDown();
            
        }

        private void loadCatagoryComboBox1()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();

            var q = from CatagoryTable in db.CatagoryTables
                    select CatagoryTable;
            
            foreach(var CatagoryTable in q)
            {
                ComboboxItem it = new ComboboxItem();
                it.Text = CatagoryTable.Name;
                it.Value = CatagoryTable.Id;
                comboBox1.Items.Add(it);
            }
        }

        private void loadDomainUpDown()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from DrugTable in db.DrugTables
                        select DrugTable;
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
            if(radioButton1.Checked)
            {
                domainUpDown1.Visible = false;
                label17.Visible = false;

                textBox1.Visible = true;
                label18.Visible = true;

                comboBox1.Visible = true;
                label16.Visible = true;

                comboBox2.Visible = false;
                label1.Visible = false;

                dateTimePicker1.Visible = false;
                label2.Visible = false;

                textBox2.Visible = false;
                label3.Visible = false;

                textBox3.Visible = false;
                label4.Visible = false;
            }
            else if(radioButton2.Checked)
            {
                domainUpDown1.Visible = true;
                label17.Visible = true;

                textBox1.Visible = true;
                label18.Visible = true;

                comboBox1.Visible = true;
                label16.Visible = true;

                comboBox2.Visible = true;
                label1.Visible = true;

                dateTimePicker1.Visible = true;
                label2.Visible = true;

                textBox2.Visible = true;
                label3.Visible = true;

                textBox3.Visible = true;
                label4.Visible = true;
            }
            else if(radioButton3.Checked)
            {
                domainUpDown1.Visible = true;
                label17.Visible = true;

                textBox1.Visible = false;
                label18.Visible = false;

                comboBox1.Visible = false;
                label16.Visible = false;

                comboBox2.Visible = false;
                label1.Visible = false;

                dateTimePicker1.Visible = false;
                label2.Visible = false;

                textBox2.Visible = false;
                label3.Visible = false;

                textBox3.Visible = false;
                label4.Visible = false;
            }
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    addNewDrug();
                }
                else if (radioButton2.Checked)
                {
                    updateDrug();
                }
                else if (radioButton3.Checked)
                {
                    discardDrug();
                }
                else MessageBox.Show("invalid input!");
            }
            catch
            {
                MessageBox.Show("invalid input!");
            }
            
        }

        private void addNewDrug()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            DrugTable dt = new DrugTable();

            try
            {
                int cataID = 0;
                bool validCata = false;
                if (comboBox1.Text != "")
                {
                    var q = from CatagoryTable in db.CatagoryTables
                            where CatagoryTable.Name == comboBox1.Text
                            select CatagoryTable;

                    foreach (var Ct in q)
                    {
                        cataID = Ct.Id;
                        validCata = true;
                        break;
                    }
                }
                var q2 = from DrugTable in db.DrugTables
                         where DrugTable.Name == textBox1.Text
                         select DrugTable;
                bool alreadyExist = false;
                foreach (var DrugTable in q2)
                {
                    alreadyExist = true;
                }
                if (alreadyExist) MessageBox.Show("Drug Already Exists \nTo make a new order please go to order page");
                else if (validCata && !alreadyExist)
                {
                    dt.Name = textBox1.Text;
                    dt.CatagoryId = cataID;
                    dt.BatchNo = 0;
                    dt.ExpiredDate = DateTime.Now;
                    dt.QuantityBought = 0;
                    dt.QuantitySold = 0;
                    dt.SellingPricePerUnit = 0;
                    dt.BuyingCostPerUnit = 0;
                    dt.Status = "valid";

                    db.DrugTables.InsertOnSubmit(dt);
                    db.SubmitChanges();

                    MessageBox.Show("Succesfully added! \nPlease Check in Orderpage to make a new order");
                    clearFields();
                }
                else
                {
                    MessageBox.Show("invalid input!");
                }
            }

            catch
            {
                MessageBox.Show("invalid input!");
            }                                 
        }

        private void discardDrug()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                DrugTable q3 = (from DrugTable in db.DrugTables
                                where DrugTable.Id == Int32.Parse(domainUpDown1.Text)
                                select DrugTable).Single();
                q3.Status = "invalid";
                db.SubmitChanges();

                MessageBox.Show("Succesfully Discarded!");
                clearFields();
            }
            catch
            {
                MessageBox.Show("invalid input!");
            }
        }

        private void updateDrug()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            DrugTable dt = new DrugTable();

            try
            {
                int cataID = 0;
                bool validCata = false;
                if (comboBox1.Text != "")
                {
                    var q = from CatagoryTable in db.CatagoryTables
                            where CatagoryTable.Name == comboBox1.Text
                            select CatagoryTable;

                    foreach (var Ct in q)
                    {
                        cataID = Ct.Id;
                        validCata = true;
                        break;
                    }
                }
                var q2 = from DrugTable in db.DrugTables
                         where DrugTable.Name == textBox1.Text
                         select DrugTable;
                bool alreadyExist = false;
                foreach (var DrugTable in q2)
                {
                    alreadyExist = true;
                    break;
                }
                if (alreadyExist) MessageBox.Show("Drug Already Exists \nTo make a new order please go to order page");
                else if (validCata && !alreadyExist)
                {
                    DrugTable q3 = (from DrugTable in db.DrugTables
                                    where DrugTable.Id == Int32.Parse(domainUpDown1.Text)
                                    select DrugTable).Single();
                    q3.Name = textBox1.Text;
                    q3.CatagoryId = cataID;
                    q3.Status = comboBox2.Text;
                    q3.ExpiredDate = DateTime.Parse(dateTimePicker1.Text);
                    q3.SellingPricePerUnit = Int32.Parse(textBox2.Text);
                    q3.BuyingCostPerUnit = Int32.Parse(textBox3.Text);

                    db.SubmitChanges();

                    MessageBox.Show("Succesfully updated! \nPlease Check in Orderpage to make a new order");
                    clearFields();
                }
                else
                {
                    MessageBox.Show("invalid input!");
                }
            }

            catch
            {
                MessageBox.Show("invalid input!");
            }                                 
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from DrugTable in db.DrugTables
                        where DrugTable.Id == Int32.Parse(domainUpDown1.Text)
                        select DrugTable;
                foreach (var nn in q)
                {
                    textBox1.Text = nn.Name;
                    comboBox1.Text = nn.CatagoryId.ToString();
                    comboBox2.Text = nn.Status;
                    dateTimePicker1.Text = nn.ExpiredDate.ToString();
                    textBox2.Text = nn.SellingPricePerUnit.ToString();
                    textBox3.Text = nn.BuyingCostPerUnit.ToString();
                    break;
                }
            }
            catch
            {
                clearFields();
            }   
        }

        private void clearFields()
        {
            textBox1.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
        }


        private void button1_Click(object sender, EventArgs e)
        {          
        }
        private void button4_Click(object sender, EventArgs e)
        {          
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }   
        private void button5_Click(object sender, EventArgs e)
        {           
        }
        private void button2_Click(object sender, EventArgs e)
        {
        }
        private void label14_Click(object sender, EventArgs e)
        {
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AddDrugs_Load(object sender, EventArgs e)
        {


        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {

        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        
        private void textBox11_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

        }





    
    }
}
