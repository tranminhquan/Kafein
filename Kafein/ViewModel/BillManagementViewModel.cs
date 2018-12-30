using Kafein.Model;
using Kafein.Model.List;
using Kafein.Model.SalesNPay;
using Kafein.Utilities;
using Kafein.View.Dialog;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.ViewModel
{
    public class BillManagementViewModel: BaseViewModel
    {
        private ListGeneralBillModel listGeneralBillModel;

        public BillManagementViewModel(): base()
        {
            listGeneralBillModel = ListGeneralBillModel.GetInstance();

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

            //listGeneralBillModel.Add(new GeneralBillModel(new BillModel("1", 2, DateTime.Now, 125000), new ListDetailBillModel()));
            //listGeneralBillModel.Add(new GeneralBillModel(new BillModel("2", 3, DateTime.Now, 12000), new ListDetailBillModel()));
            //listGeneralBillModel.Add(new GeneralBillModel(new BillModel("3", 4, DateTime.Now, 25000), new ListDetailBillModel()));
            //listGeneralBillModel.Add(new GeneralBillModel(new BillModel("4", 5, DateTime.Now, 15000), new ListDetailBillModel()));
            //listGeneralBillModel.Add(new GeneralBillModel(new BillModel("5", 6, DateTime.Now, 5000), new ListDetailBillModel()));
            //listGeneralBillModel.Add(new GeneralBillModel(new BillModel("6", 7, DateTime.Now, 15000), new ListDetailBillModel()));
            //listGeneralBillModel.Add(new GeneralBillModel(new BillModel("7", 8, DateTime.Now, 25000), new ListDetailBillModel()));


            CreateBillCommand = new DelegateCommand(CreateBill);
            DetailCommand = new DelegateCommand(ShowDetail);
            CheckoutCommand = new DelegateCommand(ShowCheckoutDialog);
        }

        public BillManagementViewModel(Action<object, object[]> navigate, object[] parameters): this()
        {
            this.navigate = navigate;
        }

        // getter and setter
        public ObservableCollection<GeneralBillModel> ListBill
        {
            get { return listGeneralBillModel.List; }
            set { listGeneralBillModel.List = value; NotifyChanged("ListBill"); }
        }

        public int SelectedIndexBill { get; set; }
        

        public DelegateCommand CreateBillCommand { get; set; }
        public DelegateCommand DetailCommand { get; set; }
        public DelegateCommand CheckoutCommand { get; set; }


        private void CreateBill()
        {
            navigate.Invoke("ListProductViewModel", null);
        }

        private void ShowDetail()
        {
            navigate.Invoke("ListProductViewModel", new object[] { SelectedIndexBill });
        }

        private void ShowCheckoutDialog()
        {
            (new CheckoutDialog(navigate, listGeneralBillModel.List[SelectedIndexBill].Bill, listGeneralBillModel.List[SelectedIndexBill].ListDetailBill.List, SelectedIndexBill)).ShowDialog();
        }
    }
}
