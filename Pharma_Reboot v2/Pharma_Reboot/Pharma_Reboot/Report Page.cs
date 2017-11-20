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
    public partial class Report_Page : Form
    {
        public Report_Page()
        {
            InitializeComponent();
            loadDatagrid1();
        }

        private void loadDatagrid1()
        {
            try
            {

                DataClasses1DataContext db = new DataClasses1DataContext();
                SalesTable dt = new SalesTable();

                var q = from SalesTable in db.SalesTables
                        select new { Drug_Name = SalesTable.DrugName, Drug_Batch_No = SalesTable.DrugTable.BatchNo, 
                            Drug_Type = SalesTable.CatagoryTable.Name, Staff_InCharge = SalesTable.UserLogIN.userName, 
                            SalesTable.QuantitySold, Date_of_Sell = SalesTable.Date, Profit_Loss_of_Sell = SalesTable.Profit_Loss };

                int tot = 0;
                foreach (var SalesTable in q)
                {
                    tot += (int)SalesTable.Profit_Loss_of_Sell;
                }
                label3.Text = "Total Profit/Loss = " + tot.ToString();


                dataGridView2.DataSource = q;
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }

        }
        private void Report_Page_Load(object sender, EventArgs e)
        {
            //loadDatagrid1();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {

                dataGridView2.Rows.Clear();
                DateTime d1 = DateTime.Parse(dateTimePicker1.Text);
                DateTime d2 = DateTime.Parse(dateTimePicker2.Text);

                DataClasses1DataContext db = new DataClasses1DataContext();
                SalesTable dt = new SalesTable();
                
                if (textBox3.Text != "")
                {
                    var q = from SalesTable in db.SalesTables
                            where SalesTable.Date >= d1 && SalesTable.Date <= d2 
                            && SalesTable.DrugName.Contains(textBox1.Text) 
                            && SalesTable.CatagoryTable.Name.Contains(textBox2.Text) 
                            && SalesTable.DrugTable.BatchNo == Int32.Parse(textBox3.Text) 
                            && SalesTable.UserLogIN.userName.Contains(textBox4.Text)
                            select new { Drug_Name = SalesTable.DrugName, Drug_Batch_No = SalesTable.DrugTable.BatchNo, Drug_Type = SalesTable.CatagoryTable.Name, Staff_InCharge = SalesTable.UserLogIN.userName, SalesTable.QuantitySold, Date_of_Sell = SalesTable.Date, Profit_Loss_of_Sell = SalesTable.Profit_Loss };
                    dataGridView2.DataSource = q;

                    int tot = 0;
                    foreach (var SalesTable in q)
                    {
                        tot += (int)SalesTable.Profit_Loss_of_Sell;
                    }
                    label3.Text = "Total Profit/Loss = " + tot.ToString();
                }
                else if (textBox3.Text == "")
                {
                    var q = from SalesTable in db.SalesTables
                            where SalesTable.Date >= d1 && SalesTable.Date <= d2 
                            && SalesTable.DrugName.Contains(textBox1.Text) 
                            && SalesTable.CatagoryTable.Name.Contains(textBox2.Text) 
                            && SalesTable.UserLogIN.userName.Contains(textBox4.Text)
                            select new { Drug_Name = SalesTable.DrugName, Drug_Batch_No = SalesTable.DrugTable.BatchNo, Drug_Type = SalesTable.CatagoryTable.Name, Staff_InCharge = SalesTable.UserLogIN.userName, SalesTable.QuantitySold, Date_of_Sell = SalesTable.Date, Profit_Loss_of_Sell = SalesTable.Profit_Loss };
                    dataGridView2.DataSource = q;

                    int tot = 0;
                    foreach (var SalesTable in q)
                    {
                        tot += (int)SalesTable.Profit_Loss_of_Sell;
                    }
                    label3.Text = "Total Profit/Loss = " + tot.ToString();
                }




                //var q = from SalesTable in db.SalesTables
                //        //where DateTime.Parse(SalesTable.Date).Date >= d1.Date //&& DateTime.Parse(SalesTable.Date) <= d2
                //        select SalesTable;

                //foreach (var SalesTable in q)
                //{
                //    DateTime d = (DateTime)SalesTable.Date;
                //    string catName = "", StaffName = "";

                //    if (d >= d1 && d <= d2)
                //    {
                //        tot += (int)SalesTable.Profit_Loss;
                //        var qC = from CatagoryTable in db.CatagoryTables
                //                 where CatagoryTable.Id == SalesTable.CatagoryID //&& DateTime.Parse(SalesTable.Date) <= d2
                //                 select CatagoryTable;

                //        foreach (var CatagoryTable in qC)
                //        {
                //            catName = CatagoryTable.Name;
                //            break;
                //        }

                //        var qS = from UserLogIN in db.UserLogINs
                //                 where UserLogIN.Id == SalesTable.StaffID //&& DateTime.Parse(SalesTable.Date) <= d2
                //                 select UserLogIN;

                //        foreach (var UserLogIN in qS)
                //        {
                //            StaffName = UserLogIN.userName;
                //            break;
                //        }

                //        this.dataGridView1.Rows.Add(SalesTable.DrugID, SalesTable.DrugName, catName, StaffName, SalesTable.QuantitySold, SalesTable.Date, SalesTable.Profit_Loss);
                //    }
                //}

                //label3.Text = "Total Profit/Loss = " + tot.ToString();

                //dataGridView1.DataSource = q;
            }
            catch
            {
                //MessageBox.Show("Invalid Input!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                dataGridView2.Rows.Clear();
                DateTime d1 = DateTime.Parse(dateTimePicker1.Text);
                DateTime d2 = DateTime.Parse(dateTimePicker2.Text);

                DataClasses1DataContext db = new DataClasses1DataContext();
                SalesTable dt = new SalesTable();

                if(textBox3.Text != "")
                {
                    var q = from SalesTable in db.SalesTables
                            where SalesTable.Date >= d1 && SalesTable.Date <= d2 
                            && SalesTable.DrugName.Contains(textBox1.Text) 
                            && SalesTable.CatagoryTable.Name.Contains(textBox2.Text) 
                            && SalesTable.DrugTable.BatchNo == Int32.Parse(textBox3.Text) 
                            && SalesTable.UserLogIN.userName.Contains(textBox4.Text)
                            select new { Drug_Name = SalesTable.DrugName, Drug_Batch_No = SalesTable.DrugTable.BatchNo, Drug_Type = SalesTable.CatagoryTable.Name, Staff_InCharge = SalesTable.UserLogIN.userName, SalesTable.QuantitySold, Date_of_Sell = SalesTable.Date, Profit_Loss_of_Sell = SalesTable.Profit_Loss };
                    dataGridView2.DataSource = q;

                    int tot = 0;
                    foreach (var SalesTable in q)
                    {
                        tot += (int)SalesTable.Profit_Loss_of_Sell;
                    }
                    label3.Text = "Total Profit/Loss = " + tot.ToString();
                }
                else
                {
                    var q = from SalesTable in db.SalesTables
                            where SalesTable.Date >= d1 && SalesTable.Date <= d2 
                            && SalesTable.DrugName.Contains(textBox1.Text) 
                            && SalesTable.CatagoryTable.Name.Contains(textBox2.Text) 
                            &&  SalesTable.UserLogIN.userName.Contains(textBox4.Text)
                            select new { Drug_Name = SalesTable.DrugName, Drug_Batch_No = SalesTable.DrugTable.BatchNo, Drug_Type = SalesTable.CatagoryTable.Name, Staff_InCharge = SalesTable.UserLogIN.userName, SalesTable.QuantitySold, Date_of_Sell = SalesTable.Date, Profit_Loss_of_Sell = SalesTable.Profit_Loss };
                    dataGridView2.DataSource = q;

                    int tot = 0;
                    foreach (var SalesTable in q)
                    {
                        tot += (int)SalesTable.Profit_Loss_of_Sell;
                    }
                    label3.Text = "Total Profit/Loss = " + tot.ToString();
                }

               
                

                //var q = from SalesTable in db.SalesTables
                //        //where DateTime.Parse(SalesTable.Date).Date >= d1.Date //&& DateTime.Parse(SalesTable.Date) <= d2
                //        select SalesTable;

                //foreach (var SalesTable in q)
                //{
                //    DateTime d = (DateTime)SalesTable.Date;
                //    string catName = "", StaffName = "";

                //    if (d >= d1 && d <= d2)
                //    {
                //        tot += (int)SalesTable.Profit_Loss;
                //        var qC = from CatagoryTable in db.CatagoryTables
                //                 where CatagoryTable.Id == SalesTable.CatagoryID //&& DateTime.Parse(SalesTable.Date) <= d2
                //                 select CatagoryTable;

                //        foreach (var CatagoryTable in qC)
                //        {
                //            catName = CatagoryTable.Name;
                //            break;
                //        }

                //        var qS = from UserLogIN in db.UserLogINs
                //                 where UserLogIN.Id == SalesTable.StaffID //&& DateTime.Parse(SalesTable.Date) <= d2
                //                 select UserLogIN;

                //        foreach (var UserLogIN in qS)
                //        {
                //            StaffName = UserLogIN.userName;
                //            break;
                //        }

                //        this.dataGridView1.Rows.Add(SalesTable.DrugID, SalesTable.DrugName, catName, StaffName, SalesTable.QuantitySold, SalesTable.Date, SalesTable.Profit_Loss);
                //    }
                //}

                //label3.Text = "Total Profit/Loss = " + tot.ToString();

                //dataGridView1.DataSource = q;
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadDatagrid1();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Open the print dialog
            
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument1;
            printDialog.UseEXDialog = true;
            //Get the document
            if (DialogResult.OK == printDialog.ShowDialog())
            {
                printDocument1.DocumentName = "Test Page Print";
                printDocument1.Print();
            }
        }
    }
}
