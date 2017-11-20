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
    public partial class ViewDrug : Form
    {
        public ViewDrug()
        {
            InitializeComponent();
        }

        private void ViewDrug_Load(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var q = from DrugTable in db.DrugTables
                    select new
                    {
                        DrugTable.Id,
                        DrugTable.Name,
                        DrugTable.CatagoryId,
                        DrugTable.BatchNo,
                        DrugTable.ExpiredDate,
                        DrugTable.QuantityBought,
                        DrugTable.QuantitySold,
                        DrugTable.SellingPricePerUnit,
                        DrugTable.BuyingCostPerUnit
                    };
            dataGridView1.DataSource = q;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string s = textBox1.Text;
            string sCol = comboBox1.Text;

            DataClasses1DataContext db = new DataClasses1DataContext();
            DrugTable dt = new DrugTable();

            try
            {
                if (sCol.ToLower() == "name")
                {
                    var q = from DrugTable in db.DrugTables
                            where DrugTable.Name == s
                            select DrugTable;

                    dataGridView1.DataSource = q;
                }
                else if (sCol.ToLower() == "batchno")
                {
                    var q = from DrugTable in db.DrugTables
                            where DrugTable.BatchNo == Int32.Parse(s)
                            select DrugTable;

                    dataGridView1.DataSource = q;
                }


                else
                {
                    MessageBox.Show("Not Found!\n");
                }

            }

            catch
            {
                MessageBox.Show("Error! Input is invalid!\n");
            }

        }
    }
}
