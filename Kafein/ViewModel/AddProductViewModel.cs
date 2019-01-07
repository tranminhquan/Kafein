using Kafein.Model;
using Kafein.Model.List;
using Kafein.View.Dialog;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kafein.ViewModel
{
    public class AddProductViewModel: BaseViewModel
    {
        private ListProductTypeModel listProductTypeModel;
        private ListUnitModel listUnitModel;
        private ProductModel updateProduct;
        private string relativePath = null;
        public AddProductViewModel(): base()
        {
            listProductTypeModel = new ListProductTypeModel();
            listProductTypeModel.LoadAllProductType();

            listUnitModel = new ListUnitModel();
            listUnitModel.LoadAllUnit();

            CancelCommand = new DelegateCommand(Cancel);
            AddProductCommand = new DelegateCommand(AddProduct);
            AddImageCommand = new DelegateCommand(ShowOpenDialog);
        }

        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public ProductModel UpdateProduct
        {
            get { return updateProduct; }
            set
            {
                updateProduct = value;
                Name = value.Name;
                Price = value.Price;
                Image = value.ImageSource;
                if (Image.Contains(Environment.CurrentDirectory))
                    relativePath = Image.Remove(0, Environment.CurrentDirectory.Length);
                else
                    relativePath = Image;
                SelectedIndexType = listProductTypeModel.GetIndexByValue("ID", value.TypeID);
                SelectedIndexUnit = listUnitModel.GetIndexByValue("ID", value.UnitID);

                NotifyChanged("UpdateProduct");
                NotifyChanged("Name");
                NotifyChanged("Price");
                NotifyChanged("Image");
                NotifyChanged("SelectedIndexType");
                NotifyChanged("SelectedIndexUnit");
            }
        }
        public ObservableCollection<object> ListType
        {
            get { return listProductTypeModel.ListName; }
        }
        public int SelectedIndexType { get; set; }
        public ObservableCollection<object> ListUnit
        {
            get { return listUnitModel.ListName; }
        }
        public int SelectedIndexUnit { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand AddProductCommand { get; set; }
        public DelegateCommand AddImageCommand { get; set; }

        private void Cancel()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "AddProductDialog")
                    window.Close();
        }

        private void AddProduct()
        {
            //check null
            if (Name == null)
            {
                (new MessageInfo("Tên mặt hàng rỗng", "Warning")).ShowDialog();
                return;
            }
                
            if (Name.Trim().Length == 0)
                return;

            if (Price <= 0)
            {
                (new MessageInfo("Chưa đặt giá tiền hoặc không đúng định dạng", "Warning")).ShowDialog();
                return;
            }
                

            // create mode
            if (updateProduct == null)
            {
                //add to database
                ProductModel product = new ProductModel(ProductModel.GenerateID(), Name, listProductTypeModel.List[SelectedIndexType].ID,
                                                    listUnitModel.List[SelectedIndexUnit].ID, Price, relativePath);
                product.SaveToDatabase();
            }
            else //update mode
            {
                
                ProductModel product = new ProductModel(updateProduct.ID, Name, listProductTypeModel.List[SelectedIndexType].ID,
                                                    listUnitModel.List[SelectedIndexUnit].ID, Price, relativePath);
                product.ImageSource = relativePath;
                ProductModel.UpdateDatabase(product);
            }

            Cancel();
        }

        private void ShowOpenDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            dialog.Title = "Please select an image";
            if (dialog.ShowDialog() == true)
            {
                // copy to relative storage
                relativePath = "\\drink_images\\" + dialog.SafeFileName;
                System.IO.File.Copy(dialog.FileName, Environment.CurrentDirectory + relativePath);
                Image = Environment.CurrentDirectory + relativePath;
                NotifyChanged("Image");
            }
        }
    }
}
