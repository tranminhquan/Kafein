using Kafein.Model;
using Kafein.Model.List;
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
            if (Name.Trim().Length == 0)
                return;
            if (Price == 0)
                return;

            //add to database
            ProductModel product = new ProductModel(ProductModel.GenerateID(), Name, listProductTypeModel.List[SelectedIndexType].ID,
                                                    listUnitModel.List[SelectedIndexUnit].ID, Price, relativePath);
            product.SaveToDatabase();

            Cancel();
        }

        private void ShowOpenDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
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
