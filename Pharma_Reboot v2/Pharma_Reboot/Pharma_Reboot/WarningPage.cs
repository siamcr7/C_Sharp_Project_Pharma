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
    public partial class WarningPage : Form
    {
        string role = "";
        int userID = 0;
        public WarningPage(string r, int uID)
        {
            userID = uID;
            InitializeComponent();
            init();
            role = r;
            label1.Text = "Viewing as " + r;

            if (r == "staff")
            {
                button1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                dataGridView2.Visible = false;
            }

        }


        void init()
        {
            loadDataGrid1();
            loadDataGrid2();
        }

        private void loadDataGrid1()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q2 = from DrugTable in db.DrugTables
                         where DrugTable.Status == "valid"
                         select DrugTable;
                int quantityLimit = 20;
                DateTime expLimit = System.DateTime.Now.AddDays(60);

                int cntNoti = 0;
                int cntExp = 0;
                foreach (var DrugTable in q2)
                {
                    int xRem = (int)DrugTable.QuantityBought - (int)DrugTable.QuantitySold;
                    DateTime expDate = (DateTime)DrugTable.ExpiredDate;

                    string diff = expDate.Subtract(DateTime.Now).ToString(@"d\d\ \:h\h\ \:m\m\ \:s\s", System.Globalization.CultureInfo.InvariantCulture);

                    if (quantityLimit > xRem)
                    {
                        cntNoti++;
                        if (expDate > DateTime.Now) this.dataGridView1.Rows.Add(DrugTable.Id, DrugTable.Name, DrugTable.CatagoryId, DrugTable.ExpiredDate, diff, DrugTable.QuantityBought, DrugTable.QuantitySold, DrugTable.QuantityBought - DrugTable.QuantitySold, DrugTable.SellingPricePerUnit, DrugTable.BuyingCostPerUnit);
                        else this.dataGridView1.Rows.Add(DrugTable.Id, DrugTable.Name, DrugTable.CatagoryId, DrugTable.ExpiredDate, "EXPIRED", DrugTable.QuantityBought, DrugTable.QuantitySold, DrugTable.QuantityBought - DrugTable.QuantitySold, DrugTable.SellingPricePerUnit, DrugTable.BuyingCostPerUnit);
                    }
                    else if (expLimit > expDate)
                    {
                        cntExp++;
                        if (expDate > DateTime.Now) this.dataGridView1.Rows.Add(DrugTable.Id, DrugTable.Name, DrugTable.CatagoryId, DrugTable.ExpiredDate, diff, DrugTable.QuantityBought, DrugTable.QuantitySold, DrugTable.QuantityBought - DrugTable.QuantitySold, DrugTable.SellingPricePerUnit, DrugTable.BuyingCostPerUnit);
                        else this.dataGridView1.Rows.Add(DrugTable.Id, DrugTable.Name, DrugTable.CatagoryId, DrugTable.ExpiredDate, "EXPIRED", DrugTable.QuantityBought, DrugTable.QuantitySold, DrugTable.QuantityBought - DrugTable.QuantitySold, DrugTable.SellingPricePerUnit, DrugTable.BuyingCostPerUnit);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }

        }

        private void loadDataGrid2()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q2 = from VendorTable in db.VendorTables
                         select VendorTable;
                dataGridView2.DataSource = q2;
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }

        }

        private void dataGrid1Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(dataGridView1.Rows[1].Cells[2].Value.ToString());
                int index = dataGridView1.CurrentCell.RowIndex;
                textBox1.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[index].Cells[7].Value.ToString();
            }
            catch
            {
                MessageBox.Show("Invalid Row Selected");
            }
        }

        private void dataGrid2Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(dataGridView1.Rows[1].Cells[2].Value.ToString());
                int index = dataGridView2.CurrentCell.RowIndex;
                textBox3.Text = dataGridView2.Rows[index].Cells[0].Value.ToString();
            }
            catch
            {
                MessageBox.Show("Invalid Row Selected");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(dataGridView1.Rows[1].Cells[2].Value.ToString());

                if (textBox1.Text != "" && textBox3.Text != "" && textBox2.Text != "")
                {
                    //int index = dataGridView1.CurrentCell.RowIndex;
                    //int index2 = dataGridView2.CurrentCell.RowIndex;
                    //textBox1.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    ////textBox2.Text = dataGridView1.Rows[index].Cells[7].Value.ToString();
                    //textBox3.Text = dataGridView1.Rows[index2].Cells[0].Value.ToString();

                    DataClasses1DataContext db = new DataClasses1DataContext();
                    OrderTable dt = new OrderTable();
                    dt.DrugID = Int32.Parse(textBox1.Text);
                    dt.QuantityToAdd = Int32.Parse(textBox2.Text);

                    if (dt.QuantityToAdd <= 0)
                    {
                        MessageBox.Show("Check Quantity to add again!");
                        dt.QuantityToAdd = 0;
                    }
                    else
                    {
                        dt.AdminID = userID;
                        //dt.OrderDate = DateTime.Now;
                        dt.VendorID = Int32.Parse(textBox3.Text);
                        dt.OrderStatus = "Pending";

                        var q = from DrugTable in db.DrugTables
                                where DrugTable.Id == Int32.Parse(textBox1.Text)
                                select DrugTable;
                        foreach (var DrugTable in q)
                        {
                            dt.DrugName = DrugTable.Name;
                            break;
                        }
                        db.OrderTables.InsertOnSubmit(dt);
                        db.SubmitChanges();

                        DrugTable q2 = (from DrugTable in db.DrugTables
                                         where DrugTable.Id == Int32.Parse(textBox1.Text)
                                        select DrugTable).Single();
                        q2.Status = "invalid";
                        db.SubmitChanges();
                        

                        MessageBox.Show("Successfully Added to Order List \nPlease Check the status in Order List");
                    }
                }
                else MessageBox.Show("Empty text box!");




            }
            catch
            {
                MessageBox.Show("Invalid Input");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            loadDataGrid1();
        }
    }
}
