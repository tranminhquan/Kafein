using Kafein.Model.List;
using Kafein.Model.SalesNPay;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.ViewModel
{
    public class BilllManagementViewModel: BaseViewModel
    {
        private ListBillModel listBillModel;

        public BilllManagementViewModel(): base()
        {
            listBillModel = new ListBillModel();

            //test
            ListBill.Add(new BillModel("1", 2, "Minh Quan", DateTime.Now, 125000));
            ListBill.Add(new BillModel("2", 12, "Minh Quan", DateTime.Now, 5000));
            ListBill.Add(new BillModel("3", 22, "Minh Quan", DateTime.Now, 75000));
            ListBill.Add(new BillModel("4", 1, "Minh Quan", DateTime.Now, 25000));
            ListBill.Add(new BillModel("4", 7, "Minh Quan", DateTime.Now, 25000));
            ListBill.Add(new BillModel("4", 20, "Minh Quan", DateTime.Now, 25000));
            ListBill.Add(new BillModel("4", 9, "Minh Quan", DateTime.Now, 25000));
            ListBill.Add(new BillModel("4", 15, "Minh Quan", DateTime.Now, 25000));
            ListBill.Add(new BillModel("4", 27, "Minh Quan", DateTime.Now, 25000));
            ListBill.Add(new BillModel("4", 14, "Minh Quan", DateTime.Now, 25000));
        }

        // getter and setter
        public ObservableCollection<BillModel> ListBill
        {
            get { return listBillModel.List; }
            set { listBillModel.List = value; NotifyChanged("ListBill"); }
        }
    }
}
