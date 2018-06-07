using Kafein.Model;
using Kafein.Model.List;
using Kafein.Model.SalesNPay;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kafein.ViewModel.Dialog
{
    public class CheckoutDialogViewModel : BaseViewModel
    {
        private BillModel bill;
        private ObservableCollection<DetailBillItemViewModel> listDetailBill;
        public CheckoutDialogViewModel() : base()
        {
            TextChangeCommand = new DelegateCommand<TextBox>(OnReceivedMoneyChange);

            CancelCommand = new DelegateCommand(Cancel);
            PrintAndCheckoutCommand = new DelegateCommand(PrintAndCheckout);
            CheckoutCommand = new DelegateCommand(Checkout);
        }

        private CheckoutDialogViewModel(Action<object, object[]> navigate, object[] parameters): this()
        {
            this.navigate = navigate;
        }

        // getter and setter
        public DelegateCommand<TextBox> TextChangeCommand { get; set; }
        public int Index { get; set; }
        public BillModel Bill
        {
            get { return bill; }
            set
            {
                bill = value;
                NotifyChanged("Bill");
                NotifyChanged("Title");
                NotifyChanged("SumPrice");
                NotifyChanged("Change");
                NotifyChanged("DeskNo");
            }
        }

        public ObservableCollection<DetailBillItemViewModel> ListDetailBill
        {
            get { return listDetailBill; }
            set
            {
                listDetailBill = value;
                NotifyChanged("ListDetailBill");
            }
        }

        public Action<object, object[]> Navigate
        {
            get { return this.navigate; }
            set { this.navigate = value; NotifyChanged("Navigate"); }
        }

        public int DeskNo
        {
            get { return bill.DeskNo; }
        }

        public double SumPrice
        {
            get { return bill.Price; }
        }
        public double MoneyReceived { get; set; }
        public double Change { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand PrintAndCheckoutCommand { get; set; }
        public DelegateCommand CheckoutCommand { get; set; }

        public void OnReceivedMoneyChange(TextBox textBox)
        {
            try
            {
                Change = Convert.ToDouble(textBox.Text) - SumPrice;
                NotifyChanged("Change");
            }
            catch(FormatException)
            {
                return;
            }
        }

        private void SaveToDatabase()
        {
            // save bill
            BillModel.SaveToDatabase(bill);

            // save detail bill
            foreach (DetailBillItemViewModel item in listDetailBill)
            {
                DetailBillModel.SaveToDatabase(item.DetailBillModel);
            }

            // if it a awaiting bill
            if (Index >= 0)
            {
                ListGeneralBillModel.GetInstance().List.Remove(ListGeneralBillModel.GetInstance().List[Index]);
                ListGeneralBillModel.GetInstance().NotifyListChange();
            }
        }

        private void Cancel()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "CheckoutDialog")
                    window.Close();
        }

        private void PrintAndCheckout()
        {
            SaveToDatabase();
            Print();
            Cancel();
        }

        private void Checkout()
        {
            SaveToDatabase();
            navigate.Invoke("BillManagementViewModel", null);
            Cancel();
        }

        private void Print()
        {

        }
    }
}
