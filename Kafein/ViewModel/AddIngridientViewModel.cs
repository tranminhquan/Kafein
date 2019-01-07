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
    public class AddIngridientViewModel : BaseViewModel
    {
        private ListUnitModel listUnitModel;
        private IngridientModel updateIngridient;
        private string relativePath = null;
        public AddIngridientViewModel() : base()
        {
            listUnitModel = new ListUnitModel();
            listUnitModel.LoadAllUnit();

            CancelCommand = new DelegateCommand(Cancel);
            AddIngridientCommand = new DelegateCommand(AddIngridient);
            AddImageCommand = new DelegateCommand(ShowOpenDialog);
        }

        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public IngridientModel UpdateIngridient
        {
            get { return updateIngridient; }
            set
            {
                updateIngridient = value;
                Name = value.Name;
                Price = value.Price;
                Image = value.ImageSource;
                if (Image.Contains(Environment.CurrentDirectory))
                    relativePath = Image.Remove(0, Environment.CurrentDirectory.Length);
                else
                    relativePath = Image;
                SelectedIndexUnit = listUnitModel.GetIndexByValue("ID", value.UnitID);

                NotifyChanged("UpdateIngridient");
                NotifyChanged("Name");
                NotifyChanged("Price");
                NotifyChanged("Image");
                NotifyChanged("SelectedIndexUnit");
            }
        }
        public ObservableCollection<object> ListUnit
        {
            get { return listUnitModel.ListName; }
        }
        public int SelectedIndexUnit { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand AddIngridientCommand { get; set; }
        public DelegateCommand AddImageCommand { get; set; }

        private void Cancel()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "AddIngridientDialog")
                    window.Close();
        }

        private void AddIngridient()
        {
            //check null
            if (Name == null)
            {
                (new MessageInfo("Tên nguyên liệu rỗng", "Warning")).ShowDialog();
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
            if (updateIngridient == null)
            {
                //add to database
                IngridientModel ingridient = new IngridientModel(IngridientModel.GenerateID(), Name, listUnitModel.List[SelectedIndexUnit].ID, Price, relativePath);
                ingridient.SaveToDatabase();
            }
            else //update mode
            {
                IngridientModel ingridient = new IngridientModel(updateIngridient.ID, Name, listUnitModel.List[SelectedIndexUnit].ID, Price, relativePath);
                ingridient.ImageSource = relativePath;
                IngridientModel.UpdateDatabase(ingridient);
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
