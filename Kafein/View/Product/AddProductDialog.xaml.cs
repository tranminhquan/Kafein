using Kafein.Model;
using Kafein.ViewModel;
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

namespace Kafein.View.Product
{
    /// <summary>
    /// Interaction logic for AddProductDialog.xaml
    /// </summary>
    public partial class AddProductDialog : Window
    {
        public AddProductDialog()
        {
            InitializeComponent();
            tbTitle.Text = "THÊM MỚI MẶT HÀNG";
            btnConfirm.Content = "THÊM MẶT HÀNG";
        }

        public AddProductDialog(ProductModel product)
        {
            InitializeComponent();
            tbTitle.Text = "CẬP NHẬT MẶT HÀNG";
            btnConfirm.Content = "CẬP NHẬT";
            ((AddProductViewModel)DataContext).UpdateProduct = product;
        }
    }
}
