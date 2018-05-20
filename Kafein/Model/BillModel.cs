using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.SalesNPay
{
    public class BillModel
    {
        // from database
        public string ID { get; set; }
        public int DeskNo { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }

        // for need
        public string OrderTime
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
        }

        public BillModel()
        {

        }

        public BillModel(string id, int deskno, string customername, DateTime date, double price)
        {
            ID = id;
            DeskNo = deskno;
            CustomerName = customername;
            Date = date;
            Price = price;
        }
    }
}
