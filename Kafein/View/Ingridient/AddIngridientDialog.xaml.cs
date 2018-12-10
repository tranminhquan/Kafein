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

namespace Kafein.View.Ingridient
{
    /// <summary>
    /// Interaction logic for AddProductDialog.xaml
    /// </summary>
    public partial class AddIngridientDialog : Window
    {
        public AddIngridientDialog()
        {
            InitializeComponent();
            tbTitle.Text = "THÊM MỚI NGUYÊN LIỆU";
            btnConfirm.Content = "THÊM NGUYÊN LIỆU";
        }

        public AddIngridientDialog(IngridientModel ingridient)
        {
            InitializeComponent();
            tbTitle.Text = "CẬP NHẬT NGUYÊN LIỆU";
            btnConfirm.Content = "CẬP NHẬT";
            ((AddIngridientViewModel)DataContext).UpdateIngridient = ingridient;
        }
    }
}
