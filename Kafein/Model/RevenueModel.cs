using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class RevenueModel
    {
        public string BillDetailID { get; set; }
        public string BillID { get; set; }
        public string ProductName { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public double Value { get; set; }
        public double Price { get; set; }

        public RevenueModel()
        {
            
        }

        public RevenueModel(RevenueModel rm)
        {
            BillDetailID = rm.BillDetailID;
            BillID = rm.BillID;
            ProductName = rm.ProductName;
            Date = rm.Date;
            Quantity = rm.Quantity;
            Value = rm.Value;
            Price = rm.Price;
        }

        public RevenueModel(string billdetailid, string billid, string productname, DateTime date, int quantity, double value, double price)
        {
            BillDetailID = billdetailid;
            BillID = billid;
            ProductName = productname;
            Date = date;
            Quantity = quantity;
            Value = value;
            Price = price;
        }
    }
}
