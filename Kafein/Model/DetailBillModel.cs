using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class DetailBillModel
    {
        public string ID { get; set; }
        public string BillID { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public DetailBillModel()
        {

        }

        public DetailBillModel(string id, string billid, string productid, int quantity, double price)
        {
            ID = id;
            billid = BillID;
            ProductID = productid;
            Quantity = quantity;
            Price = price;
        }
    }
}
