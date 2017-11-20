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
    public partial class OrderPage : Form
    {
        //private int quantityToAdd = 0;
        private string drugName = "";
        public OrderPage()
        {
            InitializeComponent();
            loadDataGridView1();
            loadComboBox();
        }

        private void loadDataGridView1()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from OrderTable in db.OrderTables
                        where OrderTable.OrderStatus == "Pending"
                        select new
                        {
                            OrderTable.Id,
                            OrderTable.DrugID,
                            OrderTable.DrugName,
                            OrderTable.QuantityToAdd,
                            OrderTable.AdminID,
                            OrderTable.OrderDate,
                            OrderTable.OrderReceiveDate,
                            OrderTable.VendorID,
                            OrderTable.OrderStatus
                        };
                dataGridView1.DataSource = q;
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        private void loadComboBox()
        {
            try
            {
                ComboboxItem ci = new ComboboxItem();
                ci.Text = "Show Full History";
                comboBox1.Items.Add(ci);

                ComboboxItem ci2 = new ComboboxItem();
                ci2.Text = "Show Pending";
                comboBox1.Items.Add(ci2);

                ComboboxItem ci3 = new ComboboxItem();
                ci3.Text = "Show Received";
                comboBox1.Items.Add(ci3);

                ComboboxItem ci4 = new ComboboxItem();
                ci4.Text = "Show Ordered";
                comboBox1.Items.Add(ci4);
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
                int index = dataGridView1.CurrentCell.RowIndex;
                //MessageBox.Show(dataGridView1.Rows[index].Cells[0].Value.ToString());
                textBox1.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                //quantityToAdd = Int32.Parse(dataGridView1.Rows[index].Cells[3].Value.ToString());
                textBox4.Text = dataGridView1.Rows[index].Cells[3].Value.ToString();
                drugName = (dataGridView1.Rows[index].Cells[2].Value.ToString());
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        private void radioButtonCheckChange(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    dateTimePicker1.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    textBox3.Visible = false;
                    textBox2.Visible = false;
                }
                else if (radioButton2.Checked)
                {
                    dateTimePicker1.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label5.Visible = true;
                    textBox3.Visible = true;
                    textBox2.Visible = true;
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        private void OrderPage_Load(object sender, EventArgs e)
        {

        }

        // refresh table
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "Show Full History")
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    var q = from OrderTable in db.OrderTables
                            select new
                            {
                                OrderTable.Id,
                                OrderTable.DrugID,
                                OrderTable.DrugName,
                                OrderTable.QuantityToAdd,
                                OrderTable.AdminID,
                                OrderTable.OrderDate,
                                OrderTable.OrderReceiveDate,
                                OrderTable.VendorID,
                                OrderTable.OrderStatus
                            };
                    dataGridView1.DataSource = q;
                }
                else if (comboBox1.Text == "Show Pending") loadDataGridView1();

                else if (comboBox1.Text == "Show Received")
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    var q = from OrderTable in db.OrderTables
                            where OrderTable.OrderStatus == "Received"
                            select new
                            {
                                OrderTable.Id,
                                OrderTable.DrugID,
                                OrderTable.DrugName,
                                OrderTable.QuantityToAdd,
                                OrderTable.AdminID,
                                OrderTable.OrderDate,
                                OrderTable.OrderReceiveDate,
                                OrderTable.VendorID,
                                OrderTable.OrderStatus
                            };
                    dataGridView1.DataSource = q;
                }

                else if (comboBox1.Text == "Show Ordered")
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    var q = from OrderTable in db.OrderTables
                            where OrderTable.OrderStatus == "Ordered"
                            select new
                            {
                                OrderTable.Id,
                                OrderTable.DrugID,
                                OrderTable.DrugName,
                                OrderTable.QuantityToAdd,
                                OrderTable.AdminID,
                                OrderTable.OrderDate,
                                OrderTable.OrderReceiveDate,
                                OrderTable.VendorID,
                                OrderTable.OrderStatus
                            };
                    dataGridView1.DataSource = q;
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }


        }

        // change button!
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    int oID = Int32.Parse(textBox1.Text);

                    DataClasses1DataContext db = new DataClasses1DataContext();
                    OrderTable q = (from OrderTable in db.OrderTables
                                    where OrderTable.Id == oID
                                    select OrderTable).Single();
                    if (radioButton1.Checked && q.OrderStatus == "Pending")
                    {
                        q.OrderStatus = "Ordered";
                        q.OrderDate = DateTime.Now;
                        q.QuantityToAdd = Int32.Parse(textBox4.Text);
                        db.SubmitChanges();
                        MessageBox.Show("Successfully Ordered");
                        loadDataGridView1();
                    }
                    else if (radioButton2.Checked && q.OrderStatus == "Ordered")
                    {
                        if (DateTime.Parse(dateTimePicker1.Text) <= DateTime.Now)
                        {
                            MessageBox.Show("Check Expired Date!");
                        }
                        else if (textBox2.Text == "" && textBox3.Text == "") MessageBox.Show("SPPU and BCPU is empty!");
                        else
                        {
                            q.OrderStatus = "Received";
                            q.OrderReceiveDate = DateTime.Now;
                            db.SubmitChanges();

                            var q2 = from DrugTable in db.DrugTables
                                     where DrugTable.Name == drugName
                                     select DrugTable;
                            int maxBatchNo = 0;
                            int cid = 0;
                            foreach (var DrugTable in q2)
                            {
                                cid = (int)DrugTable.CatagoryId;
                                if (maxBatchNo < (int)DrugTable.BatchNo) maxBatchNo = (int)DrugTable.BatchNo;
                            }
                            DrugTable dt = new DrugTable();
                            dt.Name = drugName;
                            dt.CatagoryId = cid;
                            dt.BatchNo = maxBatchNo + 1;
                            dt.QuantityBought = Int32.Parse(textBox4.Text);
                            dt.QuantitySold = 0;
                            dt.SellingPricePerUnit = Int32.Parse(textBox2.Text);
                            dt.BuyingCostPerUnit = Int32.Parse(textBox3.Text);
                            dt.ExpiredDate = DateTime.Parse(dateTimePicker1.Text);

                            db.DrugTables.InsertOnSubmit(dt);
                            db.SubmitChanges();



                            MessageBox.Show("Successfully Received\n" + q.DrugName + "\nNew Batch No :" + (maxBatchNo + 1).ToString() + "\n" + "New Quantities : " + textBox4.Text);
                            loadDataGridView1();
                        }
                    }

                }
            }
            catch
            {
                MessageBox.Show("Invalid Input");
            }

        }


        
        private void button2_Click(object sender, EventArgs e)
        {}

        // cancel order
        private void button2_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to cancel?", "Dialog Title", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (textBox1.Text != "")
                    {
                        int oID = Int32.Parse(textBox1.Text);
                        DataClasses1DataContext db = new DataClasses1DataContext();
                        var q = from OrderTable in db.OrderTables
                                where OrderTable.Id == oID
                                select OrderTable;

                        db.OrderTables.DeleteAllOnSubmit(q);
                        db.SubmitChanges();
                        MessageBox.Show("Succesfully Cancelled! \n");
                        loadDataGridView1();
                    }

                }
                catch
                {
                    MessageBox.Show("Invalid Input");
                }
            }

        }
    }
}
