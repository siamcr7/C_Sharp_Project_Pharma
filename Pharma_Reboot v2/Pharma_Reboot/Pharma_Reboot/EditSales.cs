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
    public partial class EditSales : Form
    {
        public EditSales()
        {
            InitializeComponent();
            loadSales(0);
        }

        private void loadSales(int x)
        {
            if (x == 0)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.SalesTables
                        select new
                        {
                            n.Id,
                            n.DrugID,
                            n.DrugName,
                            n.CatagoryTable.Name,
                            n.UserLogIN.FullName,
                            n.QuantitySold,
                            n.Date,
                            n.Profit_Loss
                        };
                dataGridView1.DataSource = q;
            }
            else
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.SalesTables
                        where n.Id == x
                        select new
                        {
                            n.Id,
                            n.DrugID,
                            n.DrugName,
                            n.CatagoryTable.Name,
                            n.UserLogIN.FullName,
                            n.QuantitySold,
                            n.Date,
                            n.Profit_Loss
                        };
                dataGridView1.DataSource = q;
                textBox2.Text = dataGridView1.Rows[0].Cells[5].Value.ToString();
                textBox3.Text = "0";
            }

        }

        private void EditSales_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int x = Int32.Parse(textBox1.Text);
                loadSales(x);
            }
            catch
            {
                loadSales(-1);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            textBox1.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[index].Cells[5].Value.ToString();
            textBox3.Text = "0";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadSales(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {
                int curQuantity = Int32.Parse(textBox2.Text);
                int retQuantity = Int32.Parse(textBox3.Text);

                if(retQuantity <= curQuantity)
                {
                    int newQuantity = curQuantity - retQuantity;
                    int drugID = 0;
                    string saveDrugName = "";
                    int buyCost = 0;
                    int sellPrice = 0;
                    var q = from n in db.SalesTables
                            where n.Id == Int32.Parse(textBox1.Text)
                            select n;
                    foreach(var nn in q)
                    {
                        drugID = (int)nn.DrugID;
                        saveDrugName = nn.DrugName;
                        buyCost = (int)nn.DrugTable.BuyingCostPerUnit;
                        sellPrice = (int)nn.DrugTable.SellingPricePerUnit;
                        break;
                    }

                    // update salestable -> quantity sold is dec and new profit/loss
                    SalesTable qq = (from n in db.SalesTables
                            where n.Id == Int32.Parse(textBox1.Text)
                            select n).Single();
                    qq.QuantitySold = newQuantity;
                    qq.Profit_Loss = newQuantity * sellPrice - newQuantity * buyCost;
                    db.SubmitChanges();

                    // update drugtable -> item sold decrease!
                    DrugTable qq2 = (from n in db.DrugTables
                                     where n.Id == drugID
                                     select n).Single();
                    qq2.QuantitySold -= retQuantity;
                    db.SubmitChanges();

                    string msg = "";
                    msg += "ITEM UPDATED: \n";
                    msg += "Sells ID: " + textBox1.Text + "\n";
                    msg += "Drug Name: " + saveDrugName + "\n";
                    msg += "Return Quantity: " + retQuantity.ToString() + "\n";
                    MessageBox.Show(msg);
                }
                else MessageBox.Show("Return Amount is bigger than Sold Amount");
            }
            catch
            {
                MessageBox.Show("invalid input"); 
            }
        }
    }
}
