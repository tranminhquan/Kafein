using Kafein.Model.List;
using Kafein.Model.SalesNPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class GeneralBillModel
    {
        public BillModel Bill { get; set; }
        public ListDetailBillModel ListDetailBill { get; set; }

        public GeneralBillModel()
        {
            Bill = new BillModel();
            ListDetailBill = new ListDetailBillModel();
        }

        public GeneralBillModel(BillModel bill, ListDetailBillModel list)
        {
            this.Bill = bill;
            this.ListDetailBill = list;
        }

        // getter and setter
        public int DeskNo
        {
            get { return Bill.DeskNo; }
        }
        public string OrderTime
        {
            get { return Bill.OrderTime; }
        }
        public double Price
        {
            get { return Bill.Price; }
        }
    }
}
