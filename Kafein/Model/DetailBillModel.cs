using Kafein.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class DetailBillModel
    {
        // from database
        public string ID { get; set; }
        public string BillID { get; set; }
        public string ProductID { get; set; }
        public string UnitID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        // read-only property
        public string ProductName
        {
            get
            {
                // crawl from database
                IDatabase sqldb = new SQLDatabase();
                return string.Empty; //temp
            }
        }

        public DetailBillModel()
        {

        }

        public DetailBillModel(string id, string billid, string productid, string unitid, int quantity, double price)
        {
            ID = id;
            BillID = billid;
            ProductID = productid;
            UnitID = unitid;
            Quantity = quantity;
            Price = price;
        }
    }
}
