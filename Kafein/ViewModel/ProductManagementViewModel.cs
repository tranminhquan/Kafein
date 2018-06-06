using Kafein.Model;
using Kafein.Model.List;
using Kafein.Utilities;
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

        public ProductManagementViewModel(): base()
        {
            listProductModel = ListProductModel.GetInstance();
            listProductModel.LoadAllProduct();
            bufferList = new ObservableCollection<ProductModel>(listProductModel.List);

            SelectedProduct = new ProductModel();
            SelectedProduct = ListProduct[0];
            NotifyProductChange();

            ProductSelectionChangeCommand = new DelegateCommand<ProductModel>(ProductChange);
            SearchTextChangeCommand = new DelegateCommand<TextBox>(OnSearchTextChange);
            AddProductCommand = new DelegateCommand(ShowAddProductDialog);
        }

        public ProductManagementViewModel(Action<object, object[]> navigate, object[] parameters): this()
        {
            this.navigate = navigate;
        }

        // getter and setter
        public ObservableCollection<ProductModel> ListProduct
        {
            get { return listProductModel.List; }
            set { listProductModel.List = value; NotifyChanged("ListProduct"); }
        }

        public ProductModel SelectedProduct { get; set; }

        public DelegateCommand<ProductModel> ProductSelectionChangeCommand { get; set; }
        public DelegateCommand<TextBox> SearchTextChangeCommand { get; set; }
        public DelegateCommand AddProductCommand { get; set; }

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

            var listMatch = from product in bufferList
                            where product.Name.ToLower().Contains(textBox.Text.ToLower())
                            select new ProductModel(product);

            ListProduct.Clear();
            ListProduct = new ObservableCollection<ProductModel>(listMatch);
        }
    }
}
