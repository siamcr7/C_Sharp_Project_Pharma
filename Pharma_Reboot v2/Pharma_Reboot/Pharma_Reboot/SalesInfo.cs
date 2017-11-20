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
    public partial class SalesInfo : Form
    {
        private int rowCount = 0;
        private int totalCost = 0;
        private Form par;

        int userID = 0;

        // these will be used to update the table after sell
        int[] arrSoldID = new int[1000];
        int[] arrSoldQuantity = new int[1000];
        int[] arrSoldQuantityChk = new int[1000];


        // to keep track of opened forms
        Form[] arrTrackOfOpenedForm = new Form[1000];
        int arrTrackIndex = 0;

        public SalesInfo(int uid, string uname, Form p)
        {
            userID = uid;
            InitializeComponent();
            init();
            loadWarningNotification();
            label4.Text = "Logged in as : " + uname;
            par = p;
        }

        void loadWarningNotification()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                label9.Text = "0 WARNINGS!";
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
                label9.Text = cntNoti.ToString() + " WARNINGS!";
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }


        private void refreshDataGrid2()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from DrugTable in db.DrugTables
                        where DrugTable.Status == "valid"
                        select new { DrugTable.Id, DrugTable.Name, DrugTable.BatchNo, CatagoryName = DrugTable.CatagoryTable.Name, DrugTable.ExpiredDate, Quantity_Rem = DrugTable.QuantityBought - DrugTable.QuantitySold, DrugTable.SellingPricePerUnit };
                dataGridView2.DataSource = q;
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        private void createComboBox()
        {
            try
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var qC = from CatagoryTable in db.CatagoryTables
                         select CatagoryTable;
                foreach (var CatagoryTable in qC)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = CatagoryTable.Name;
                    //item.Value = CatagoryTable.id;
                    comboBox1.Items.Add(item);
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }

        }


        private void init()
        {
            try
            {
                refreshDataGrid2();
                createComboBox();

                for (int i = 0; i < 1000; i++)
                {
                    arrSoldID[i] = -1;
                    arrSoldQuantityChk[i] = 0;
                }


                //this.dataGridView1.Rows.Add("five", "six", "seven", "eight");
                //this.dataGridView1.Rows.Add("five", "six", "sevedddn", "eight");

                //MessageBox.Show(dataGridView1.Rows[2].Cells[3].Value.ToString());

                //this.dataGridView1.Rows.Insert(0, "one", "two", "three", "four");
                //this.dataGridView1.Rows.Insert(0, "one", "two", "three", "frrour");

                //MessageBox.Show(dataGridView1.Rows[1].Cells[2].Value.ToString());
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
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
                                         where UserLogIN.Id == userID
                                         select UserLogIN).Single();
                        q22.LastLogoutTime = DateTime.Now;
                        db.SubmitChanges();
                        MessageBox.Show("You are Logged out\nLast LoginTime : " + q22.LastLoginTime.ToString() + "\nLast LogOut Time : " + q22.LastLogoutTime.ToString());

                        SessionTable dt = new SessionTable();
                        dt.UserID = userID;
                        dt.LogInTime = (DateTime)q22.LastLoginTime;
                        dt.LogOutTime = DateTime.Now;
                        db.SessionTables.InsertOnSubmit(dt);
                        db.SubmitChanges();

                        par.Visible = true;

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


        private void dataGrid2Click(object sender, EventArgs e)
        {
            int index = dataGridView2.CurrentCell.RowIndex;
            textBox2.Text = dataGridView2.Rows[index].Cells[0].Value.ToString();
        }

        private void SalesInfo_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // add to recipt
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (textBox2.Text != "" && textBox4.Text != "")
                {
                    int dID = Int32.Parse(textBox2.Text);
                    int quantity = Int32.Parse(textBox4.Text);

                    DataClasses1DataContext db = new DataClasses1DataContext();
                    DrugTable dt = new DrugTable();

                    var q = from DrugTable in db.DrugTables
                            where DrugTable.Id == dID
                            select DrugTable;

                    foreach (var DrugTable in q)
                    {
                        if (quantity <= DrugTable.QuantityBought - DrugTable.QuantitySold && (DateTime)DrugTable.ExpiredDate > System.DateTime.Today && arrSoldQuantityChk[DrugTable.Id] + quantity <= DrugTable.QuantityBought - DrugTable.QuantitySold)
                        {
                            this.dataGridView1.Rows.Add(rowCount + 1, DrugTable.Name, quantity, DrugTable.SellingPricePerUnit, DrugTable.ExpiredDate, DrugTable.BatchNo);
                            arrSoldID[rowCount + 1] = DrugTable.Id;
                            arrSoldQuantity[rowCount + 1] = quantity;
                            arrSoldQuantityChk[DrugTable.Id] += quantity;
                            rowCount++;
                            totalCost += quantity * (int)DrugTable.SellingPricePerUnit;
                            label6.Text = "Total Cost = " + totalCost.ToString();
                        }
                        else if (quantity > DrugTable.QuantityBought - DrugTable.QuantitySold || arrSoldQuantityChk[DrugTable.Id] + quantity > DrugTable.QuantityBought - DrugTable.QuantitySold)
                        {
                            MessageBox.Show("Not Enough Quantity Remaining!");
                        }
                        else if ((DateTime)DrugTable.ExpiredDate <= System.DateTime.Today)
                        {
                            MessageBox.Show("Date Expired!!!");
                        }

                        break;
                    }
                }

            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        // name srch extendend!
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string srchStr = textBox1.Text;

                if (srchStr != "")
                {
                    //http://stackoverflow.com/questions/1202981/select-multiple-fields-from-list-in-linq
                    DataClasses1DataContext db = new DataClasses1DataContext();
                    var q = from DrugTable in db.DrugTables
                            where DrugTable.Name.Contains(srchStr) && DrugTable.QuantityBought - DrugTable.QuantitySold > 0 && DateTime.Now.Date < DrugTable.ExpiredDate
                            //join c in CatagoryTable on c.Id equals DrugTable.CatagoryId
                            select new { DrugTable.Id, DrugTable.Name, DrugTable.BatchNo, CatagoryName = DrugTable.CatagoryTable.Name, DrugTable.ExpiredDate, Quantity_Rem = DrugTable.QuantityBought - DrugTable.QuantitySold, DrugTable.SellingPricePerUnit };

                    dataGridView2.DataSource = q;
                }

            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        // catagory srch
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string srchStr = comboBox1.Text;
                if (srchStr != "")
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();

                    var qC = from CatagoryTable in db.CatagoryTables
                             where CatagoryTable.Name == srchStr
                             select CatagoryTable;
                    int cataID = 0;
                    foreach (var CatagoryTable in qC)
                    {
                        cataID = CatagoryTable.Id;
                        break;
                    }

                    var q = from DrugTable in db.DrugTables
                            where DrugTable.CatagoryId == cataID && DrugTable.QuantityBought - DrugTable.QuantitySold > 0 && DateTime.Now.Date < DrugTable.ExpiredDate
                            //join c in CatagoryTable on c.Id equals DrugTable.CatagoryId
                            select new { DrugTable.Id, DrugTable.Name, DrugTable.BatchNo, CatagoryName = DrugTable.CatagoryTable.Name, DrugTable.ExpiredDate, Quantity_Rem = DrugTable.QuantityBought - DrugTable.QuantitySold, DrugTable.SellingPricePerUnit };

                    dataGridView2.DataSource = q;
                }

            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }

        }

        // name + cata srch
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string srchStr2 = textBox1.Text;
                string srchStr = comboBox1.Text;
                if (srchStr != "" && srchStr2 != "")
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();

                    var qC = from CatagoryTable in db.CatagoryTables
                             where CatagoryTable.Name == srchStr
                             select CatagoryTable;
                    int cataID = 0;
                    foreach (var CatagoryTable in qC)
                    {
                        cataID = CatagoryTable.Id;
                        break;
                    }

                    var q = from DrugTable in db.DrugTables
                            where DrugTable.CatagoryId == cataID && DrugTable.Name.Contains(srchStr2) && DrugTable.QuantityBought - DrugTable.QuantitySold > 0 && DateTime.Now.Date < DrugTable.ExpiredDate
                            //join c in CatagoryTable on c.Id equals DrugTable.CatagoryId
                            select new { DrugTable.Id, DrugTable.Name, DrugTable.BatchNo, CatagoryName = DrugTable.CatagoryTable.Name, DrugTable.ExpiredDate, Quantity_Rem = DrugTable.QuantityBought - DrugTable.QuantitySold, DrugTable.SellingPricePerUnit };

                    dataGridView2.DataSource = q;
                }

            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }


        // calc return
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {

                if (textBox5.Text != "")
                {
                    int ga = Int32.Parse(textBox5.Text);
                    label8.Text = "Return Amount: " + (ga - totalCost).ToString();
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        // delete row
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int index = dataGridView1.CurrentCell.RowIndex;

                //MessageBox.Show(dataGridView1.Rows[1].Cells[2].Value.ToString());

                int rowCnt = Int32.Parse(dataGridView1.Rows[index].Cells[0].Value.ToString());
                
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from DrugTable in db.DrugTables
                         where DrugTable.Name == dataGridView1.Rows[index].Cells[1].Value.ToString() && DrugTable.BatchNo == Int32.Parse(dataGridView1.Rows[index].Cells[5].Value.ToString())
                                 select DrugTable).Single();
                arrSoldQuantityChk[q.Id] -= Int32.Parse(dataGridView1.Rows[index].Cells[2].Value.ToString());
                arrSoldID[rowCnt] = -1;

                int quantity = Int32.Parse(dataGridView1.Rows[index].Cells[2].Value.ToString());
                int ppu = Int32.Parse(dataGridView1.Rows[index].Cells[3].Value.ToString());

                totalCost -= quantity * ppu;
                label6.Text = "Total Cost = " + totalCost.ToString();

                dataGridView1.Rows.RemoveAt(index);
            }
            catch
            {
                MessageBox.Show("Invalid Row Selected");
            }

            //MessageBox.Show(index.ToString());

        }

        // clr datagrid1
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                rowCount = 0;
                totalCost = 0;
                label6.Text = "Total Cost = 0";
                label8.Text = "Return Amount: 0";
                dataGridView1.Rows.Clear();
                for (int i = 0; i < 1000; i++)
                {
                    arrSoldID[i] = -1;
                    arrSoldQuantityChk[i] = 0;
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }
        }

        // sell button aka update drugtable and add to salesTable
        private void button8_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {
                bool sold = false;
                int prvSaleID = 0;
                for (int i = 0; i < 1000; i++)
                {
                    if (arrSoldID[i] != -1)
                    {
                        if (!sold)
                        {
                            var q22 = from n in db.SalesTables
                                      select n;
                            foreach (var nn in q22) prvSaleID = nn.Id;
                        }
                        sold = true;

                        // updating drugtable
                        DrugTable q = (from DrugTable in db.DrugTables
                                       where DrugTable.Id == arrSoldID[i]
                                       select DrugTable).Single();
                        q.QuantitySold += arrSoldQuantity[i];
                        db.SubmitChanges();


                        // adding to sales table
                        var q2 = from DrugTable in db.DrugTables
                                 where DrugTable.Id == arrSoldID[i]
                                 select DrugTable;
                        foreach (var DrugTable in q2)
                        {
                            SalesTable st = new SalesTable();
                            st.DrugID = DrugTable.Id;
                            st.DrugName = DrugTable.Name;
                            st.CatagoryID = DrugTable.CatagoryId;
                            st.QuantitySold = arrSoldQuantity[i];
                            st.Date = System.DateTime.Now;
                            st.Profit_Loss = (DrugTable.SellingPricePerUnit - DrugTable.BuyingCostPerUnit) * arrSoldQuantity[i];
                            st.StaffID = userID;

                            db.SalesTables.InsertOnSubmit(st);
                            db.SubmitChanges();

                            break;
                        }
                    }
                }
                if (sold)
                {
                    var q22 = from n in db.SalesTables
                              where n.Id > prvSaleID
                              select n;
                    string s = "";

                    foreach(var nn in q22)
                    {
                        if(nn.Id == prvSaleID+1)
                        {
                            s += "SOLD ITEMS: \n";
                        }
                        s += "Sell ID: " + (nn.Id).ToString() + "\n";
                        s += "DrugName: "+ nn.DrugName + "\n";
                        s += "Quantity: " + nn.QuantitySold + "\n";
                        s += "\n\n";                        
                    }
                    s += label6.Text;
                    MessageBox.Show(s);
                    refreshDataGrid2();
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input!");
            }

        }


        


        private void button9_Click(object sender, EventArgs e)
        {
            refreshDataGrid2();
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label9.Cursor = Cursors.Hand;
        }

        private void label9_Click(object sender, EventArgs e)
        {
            WarningPage fm = new WarningPage("staff", userID);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            StaffProfile fm = new StaffProfile(userID);
            fm.Show();
            arrTrackOfOpenedForm[arrTrackIndex] = fm;
            arrTrackIndex++;
        }

        private void editSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSales fm = new EditSales();
            fm.Show();
        }
    }

    /// already defined in adminMenu
    //public class ComboboxItem
    //{
    //    public string Text { get; set; }
    //    public object Value { get; set; }

    //    public override string ToString()
    //    {
    //        return Text;
    //    }
    //}

}
