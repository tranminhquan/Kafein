using Kafein.View.Dialog;
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
    /// Interaction logic for SearchFilterDialog.xaml
    /// </summary>
    public partial class SearchFilterDialog : Window
    {
        public string Field { get; set; }
        public string Sort { get; set; }
        public SearchFilterDialog()
        {
            InitializeComponent();
            Field = "Name";
            Sort = null;
        }

        private void rbName_Checked(object sender, RoutedEventArgs e)
        {
            Field = "Name";
        }

        private void rbPrice_Checked(object sender, RoutedEventArgs e)
        {
            Field = "Price";
        }

        private void rbASC_Checked(object sender, RoutedEventArgs e)
        {
            Sort = "ASC";
        }

        private void rbDESC_Checked(object sender, RoutedEventArgs e)
        {
            Sort = "DESC";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Field = null;
            Sort = null;
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "SearchFilterDialog")
                    window.Close();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "SearchFilterDialog")
                    window.Close();
        }
    }
}
