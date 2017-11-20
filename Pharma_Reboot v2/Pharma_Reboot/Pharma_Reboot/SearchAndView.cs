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
    public partial class SearchAndView : Form
    {
        string s = "";
        public SearchAndView(string s)
        {
            InitializeComponent();
            this.s = s;
            label1.Text = s;
            if(s.Contains("Catagory"))
            {
                loadCatagory(0);
            }
            else if(s.Contains("Drug"))
            {
                loadDrug(0);
            }
            else if (s.Contains("Order"))
            {
                loadOrder(0);
            }
            else if (s.Contains("Payment"))
            {
                loadPayment(0);
            }
            else if (s.Contains("Sales"))
            {
                loadSales(0);
            }
            else if (s.Contains("Session"))
            {
                loadSession(0);
            }
            else if (s.Contains("Vendor"))
            {
                loadVendor(0);
            }
        }

        private void loadCatagory(int x)
        {
            if (x == 0)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.CatagoryTables
                        select new
                        {
                            n.Id,
                            n.Name
                        };
                dataGridView1.DataSource = q;
            }
            else
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.CatagoryTables
                        where n.Id == x
                        select new
                        {
                            n.Id,
                            n.Name
                        };
                dataGridView1.DataSource = q;
            }
            
        }

        private void loadDrug(int x)
        {
            if (x == 0)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.DrugTables
                        select new
                        {
                            n.Id,
                            n.Name,
                            n.CatagoryId,
                            n.BatchNo,
                            n.ExpiredDate,
                            n.QuantityBought,
                            n.QuantitySold,
                            n.SellingPricePerUnit,
                            n.BuyingCostPerUnit,
                            n.Status
                        };
                dataGridView1.DataSource = q;
            }
            else
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.DrugTables
                        where n.Id == x
                        select new
                        {
                            n.Id,
                            n.Name,
                            n.CatagoryId,
                            n.BatchNo,
                            n.ExpiredDate,
                            n.QuantityBought,
                            n.QuantitySold,
                            n.SellingPricePerUnit,
                            n.BuyingCostPerUnit,
                            n.Status
                        };
                dataGridView1.DataSource = q;
            }
            
        }

        private void loadOrder(int x)
        {
            if (x == 0)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.OrderTables
                        select new
                        {
                            n.Id,
                            n.DrugID,
                            n.DrugName,
                            n.QuantityToAdd,
                            n.AdminID,
                            n.OrderDate,
                            n.OrderReceiveDate,
                            n.VendorID,
                            n.OrderStatus
                        };
                dataGridView1.DataSource = q;
            }
            else
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.OrderTables
                        where n.Id == x
                        select new
                        {
                            n.Id,
                            n.DrugID,
                            n.DrugName,
                            n.QuantityToAdd,
                            n.AdminID,
                            n.OrderDate,
                            n.OrderReceiveDate,
                            n.VendorID,
                            n.OrderStatus
                        };
                dataGridView1.DataSource = q;
            }
            
        }

        private void loadPayment(int x)
        {
            if (x == 0)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.PaymentTables
                        select new
                        {
                            n.Id,
                            n.UserId,
                            n.DateOfPayment,
                            n.PaymentAmount,
                            n.ExtraBonus
                        };
                dataGridView1.DataSource = q;
            }
            else
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.PaymentTables
                        where n.Id == x
                        select new
                        {
                            n.Id,
                            n.UserId,
                            n.DateOfPayment,
                            n.PaymentAmount,
                            n.ExtraBonus
                        };
                dataGridView1.DataSource = q;
            }
            
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
                            n.CatagoryID,
                            n.StaffID,
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
                            n.CatagoryID,
                            n.StaffID,
                            n.QuantitySold,
                            n.Date,
                            n.Profit_Loss
                        };
                dataGridView1.DataSource = q;
            }
            
        }

        private void loadSession(int x)
        {
            if (x == 0)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.SessionTables
                        select new
                        {
                            n.Id,
                            n.UserID,
                            n.LogInTime,
                            n.LogOutTime
                        };
                dataGridView1.DataSource = q;
            }
            else
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.SessionTables
                        where n.Id == x
                        select new
                        {
                            n.Id,
                            n.UserID,
                            n.LogInTime,
                            n.LogOutTime
                        };
                dataGridView1.DataSource = q;
            }
            
        }

        private void loadVendor(int x)
        {
            if(x == 0)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.VendorTables
                        select new
                        {
                            n.Id,
                            n.Name,
                            n.Address,
                            n.Phone,
                            n.Email
                        };
                dataGridView1.DataSource = q;
            }
            else
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = from n in db.VendorTables
                        where n.Id == x
                        select new
                        {
                            n.Id,
                            n.Name,
                            n.Address,
                            n.Phone,
                            n.Email
                        };
                dataGridView1.DataSource = q;
            }
            
        }

        private void multiSearch(int index)
        {
            if (s.Contains("Catagory"))
            {
                loadCatagory(index);
            }
            else if (s.Contains("Drug"))
            {
                loadDrug(index);
            }
            else if (s.Contains("Order"))
            {
                loadOrder(index);
            }
            else if (s.Contains("Payment"))
            {
                loadPayment(index);
            }
            else if (s.Contains("Sales"))
            {
                loadSales(index);
            }
            else if (s.Contains("Session"))
            {
                loadSession(index);
            }
            else if (s.Contains("Vendor"))
            {
                loadVendor(index);
            }
        }

        private void SearchAndView_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int x = Int32.Parse(textBox1.Text);
                multiSearch(x);
            }
            catch
            {
                multiSearch(-1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            multiSearch(0);
        }

       
        
    }
}
