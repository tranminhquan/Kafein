using Kafein.Model;
using Kafein.Model.SalesNPay;
using Kafein.ViewModel.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kafein.View.Dialog
{
    /// <summary>
    /// Interaction logic for CheckoutDialog.xaml
    /// </summary>
    public partial class CheckoutDialog : Window
    {
        public CheckoutDialog()
        {
            InitializeComponent();
        }

        public CheckoutDialog(BillModel bill, DetailBillModel detail): this()
        {
            ((CheckoutDialogViewModel)DataContext).Bill = bill;
            ((CheckoutDialogViewModel)DataContext).DetailBill = detail;
        }
    }
}
