using Kafein.Model;
using Kafein.Model.List;
using Kafein.Utilities;
using Kafein.View.Dialog;
using Kafein.View.Product;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kafein.ViewModel
{
    public class ProductManagementViewModel: BaseViewModel
    {
        private ListProductModel listProductModel;
        private ObservableCollection<ProductModel> bufferList;
        private string fieldSearch = null;
        private string sortSearch = null;

        public ProductManagementViewModel(): base()
        {
            listProductModel = ListProductModel.GetInstance();
            listProductModel.LoadAllProduct();
            bufferList = new ObservableCollection<ProductModel>(listProductModel.List);

            SelectedProduct = new ProductModel();
            SelectedProduct = ListProduct[0];
            NotifyProductChange();

            fieldSearch = "Name";

            ProductSelectionChangeCommand = new DelegateCommand<ProductModel>(ProductChange);
            SearchTextChangeCommand = new DelegateCommand<TextBox>(OnSearchTextChange);
            AddProductCommand = new DelegateCommand(ShowAddProductDialog);
            UpdateProductCommand = new DelegateCommand(UpdateProduct);
            RemoveProductCommand = new DelegateCommand(RemoveProduct);
            OpenSearchFilterCommand = new DelegateCommand(ShowSearchFilterDialog);
        }

        public ProductManagementViewModel(Action<object, object[]> navigate, object[] parameters): this()
        {
            this.navigate = navigate;
        }

        // getter and setter
        public ObservableCollection<ProductModel> ListProduct
        {
            get { return listProductModel.List; }
            set
            {
                listProductModel.List = value;
                //SelectedProduct = ListProduct[0];
                NotifyChanged("ListProduct");
                NotifyProductChange();
            }
        }

        public ProductModel SelectedProduct { get; set; }

        public DelegateCommand<ProductModel> ProductSelectionChangeCommand { get; set; }
        public DelegateCommand<TextBox> SearchTextChangeCommand { get; set; }
        public DelegateCommand AddProductCommand { get; set; }
        public DelegateCommand UpdateProductCommand { get; set; }
        public DelegateCommand RemoveProductCommand { get; set; }
        public DelegateCommand OpenSearchFilterCommand { get; set; }

        private void ProductChange(ProductModel product)
        {
            NotifyProductChange();
        }

        private void NotifyProductChange()
        {
            NotifyChanged("Name");
            NotifyChanged("Type");
            NotifyChanged("Unit");
            NotifyChanged("Price");
            NotifyChanged("Image");
            NotifyChanged("MaxSaleProduct");
            NotifyChanged("SaleProduct");
            NotifyChanged("Popular");
            NotifyChanged("SelectedProduct");
        }

        public string Name
        {
            get { return SelectedProduct.Name; }
        }

        public string Type
        {
            get { return ProductTypeModel.GetModelFromID(SelectedProduct.TypeID).Name; }
        }

        public string Unit
        {
            get { return UnitModel.GetModelFromID(SelectedProduct.UnitID).Name; }
        }

        public string Price
        {
            get { return SelectedProduct.Price.ToString(); }
        }

        public string Image
        {
            get { return SelectedProduct.ImageSource; }
        }

        public int MaxSaleProduct
        {
            get { return ListDetailBillModel.GetSumDetailBill(); }
        }

        public int SaleProduct
        {
            get { return ListDetailBillModel.GetSumDetailBillFromProduct(SelectedProduct.ID); }
        }

        public string Popular
        {
            get
            {
                double p = Convert.ToDouble(SaleProduct) / Convert.ToDouble(MaxSaleProduct);
                return (Math.Round(p, 2) * 100).ToString() + "%";
            }
        }

        private void ShowAddProductDialog()
        {
            (new AddProductDialog()).ShowDialog();

            //reload list product
            listProductModel.LoadAllProduct();
            bufferList = listProductModel.List;
        }

        private void OnSearchTextChange(TextBox textBox)
        {
            if (textBox.Text.Length == 0)
            {
                listProductModel.LoadAllProduct();
                NotifyProductChange();
                return;
            }

            IEnumerable<ProductModel> listMatch = null;
            if (fieldSearch == "Name")
            {
                 listMatch = from product in bufferList
                             where product.Name.ToLower().Contains(textBox.Text.ToLower())
                             select new ProductModel(product);              
            }
            else if (fieldSearch == "Price" && textBox.Text.Length >= 3)
            {

                if (textBox.Text.Contains(">="))
                {
                    double compare = Convert.ToDouble(textBox.Text.Remove(0, 2));
                    listMatch = from product in bufferList
                                where product.Price >= compare
                                select new ProductModel(product);
                }
                else if (textBox.Text.Contains("<="))
                {
                    double compare = Convert.ToDouble(textBox.Text.Remove(0, 2));
                    listMatch = from product in bufferList
                                where product.Price <= compare
                                select new ProductModel(product);
                }
                else if (textBox.Text.Contains(">"))
                {
                    double compare = Convert.ToDouble(textBox.Text.Remove(0, 1));
                    listMatch = from product in bufferList
                                where product.Price > compare
                                select new ProductModel(product);
                } 
                else if (textBox.Text.Contains("<"))
                {
                    double compare = Convert.ToDouble(textBox.Text.Remove(0, 1));
                    listMatch = from product in bufferList
                                where product.Price < compare
                                select new ProductModel(product);
                }              
                else
                {
                    double compare;
                    if (textBox.Text.Contains("="))
                        compare = Convert.ToDouble(textBox.Text.Remove(0, 1));
                    else
                        compare = Convert.ToDouble(textBox.Text);

                    listMatch = from product in bufferList
                                where product.Price == compare
                                select new ProductModel(product);
                }
            }

            if (listMatch == null)
                return;

            if (sortSearch != null)
            {
                if (sortSearch == "ASC")
                {
                    listMatch = listMatch.OrderBy(product => product.GetType().GetProperty(fieldSearch));
                }
                if (sortSearch == "DESC")
                {
                    listMatch = listMatch.OrderByDescending(product => product.GetType().GetProperty(fieldSearch));
                }
            }

            ListProduct.Clear();
            ListProduct = new ObservableCollection<ProductModel>(listMatch);
        }

        private void UpdateProduct()
        {
            (new AddProductDialog(SelectedProduct)).ShowDialog();
            listProductModel.LoadAllProduct();
            bufferList = listProductModel.List;
        }

        private void RemoveProduct()
        {
            ConfirmDialog confirmDialog = 
                new ConfirmDialog("CẢNH BÁO", "Mặt hàng này sẽ không tồn tại trong hệ thống nếu tiếp tục. Xác nhận xóa mặt hàng?",
                 (Action)delegate
                 {
                     ProductModel.RemoveFromDatabase(SelectedProduct.ID);
                 });
            confirmDialog.ShowDialog();
            listProductModel.LoadAllProduct();
            bufferList = listProductModel.List;
        }

        private void ShowSearchFilterDialog()
        {
            SearchFilterDialog searchFilterDialog = new SearchFilterDialog();
            searchFilterDialog.ShowDialog();
            fieldSearch = searchFilterDialog.Field;
            sortSearch = searchFilterDialog.Sort;
        }
    }
}
