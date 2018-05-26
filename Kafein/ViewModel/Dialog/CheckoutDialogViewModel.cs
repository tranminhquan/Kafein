using Kafein.Model;
using Kafein.Model.SalesNPay;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kafein.ViewModel.Dialog
{
    public class CheckoutDialogViewModel : BaseViewModel
    {
        private BillModel bill;
        private DetailBillModel detail;
        public CheckoutDialogViewModel() : base()
        {
            TextChangeCommand = new DelegateCommand<TextBox>(OnReceivedMoneyChange);
        }

        // getter and setter
        public DelegateCommand<TextBox> TextChangeCommand { get; set; }
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
            }
        }

        public DetailBillModel DetailBill
        {
            get { return detail; }
            set
            {
                detail = value;
                NotifyChanged("DetailBill");
            }
        }

        public string Title
        {
            get { return "THANH TOÁN BÀN " + bill.DeskNo; }
        }

        public double SumPrice
        {
            get { return bill.Price; }
        }
        public double MoneyReceived { get; set; }
        public double Change { get; set; }

        public void OnReceivedMoneyChange(TextBox textBox)
        {
            Change = Convert.ToDouble(textBox.Text) - SumPrice;
            NotifyChanged("Change");
        }
    }
}
