using Kafein.Model.List;
using Kafein.Model.SalesNPay;
using Kafein.Utilities;
using Prism.Commands;
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
            listBillModel = ListBillModel.GetInstace();

            ////test
            //ListBill.Add(new BillModel("1", 2, DateTime.Now, 125000));
            //ListBill.Add(new BillModel("2", 12, DateTime.Now, 5000));
            //ListBill.Add(new BillModel("3", 22, DateTime.Now, 75000));
            //ListBill.Add(new BillModel("4", 1, DateTime.Now, 25000));
            //ListBill.Add(new BillModel("4", 7, DateTime.Now, 25000));
            //ListBill.Add(new BillModel("4", 20, DateTime.Now, 25000));
            //ListBill.Add(new BillModel("4", 9, DateTime.Now, 25000));
            //ListBill.Add(new BillModel("4", 15, DateTime.Now, 25000));
            //ListBill.Add(new BillModel("4", 27, DateTime.Now, 25000));
            //ListBill.Add(new BillModel("4", 14, DateTime.Now, 25000));

            CreateBillCommand = new DelegateCommand(CreateBill);
        }

        public BilllManagementViewModel(Action<object, object[]> navigate, object[] parameters): this()
        {
            this.navigate = navigate;
        }

        // getter and setter
        public ObservableCollection<BillModel> ListBill
        {
            get { return listBillModel.List; }
            set { listBillModel.List = value; NotifyChanged("ListBill"); }
        }

        public DelegateCommand CreateBillCommand { get; set; }


        public void CreateBill()
        {
            navigate.Invoke("ListProductViewModel", null);  
        }
    }
}
