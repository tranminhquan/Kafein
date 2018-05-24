using Kafein.Model.SalesNPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListBillModel: BaseList<BillModel>
    {
        private static ListBillModel instance = null;
        public ListBillModel(): base()
        {

        }

        public static ListBillModel GetInstace()
        {
            if (instance == null)
                instance = new ListBillModel();
            return instance;
        }
    }
}
