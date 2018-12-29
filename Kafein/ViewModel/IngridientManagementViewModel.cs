using Kafein.Model;
using Kafein.Model.List;
using Kafein.Utilities;
using Kafein.View.Dialog;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Kafein.View.Ingridient;

namespace Kafein.ViewModel
{
    public class IngridientManagementViewModel : BaseViewModel
    {
        private ListIngridientModel listIngridientModel;
        private ObservableCollection<IngridientModel> bufferList;
        private string fieldSearch = null;
        private string sortSearch = null;

        public IngridientManagementViewModel() : base()
        {
            listIngridientModel = ListIngridientModel.GetInstance();
            listIngridientModel.LoadAllIngridient();
            bufferList = new ObservableCollection<IngridientModel>(listIngridientModel.List);

            SelectedIngridient = new IngridientModel();
            SelectedIngridient = ListIngridient[0];
            NotifyProductChange();

            fieldSearch = "Name";

            IngridientSelectionChangeCommand = new DelegateCommand<IngridientModel>(IngridientChange);
            SearchTextChangeCommand = new DelegateCommand<TextBox>(OnSearchTextChange);
            AddIngridientCommand = new DelegateCommand(ShowAddIngridientDialog);
            UpdateIngridientCommand = new DelegateCommand(UpdateIngridient);
            RemoveIngridientCommand = new DelegateCommand(RemoveIngridient);
            OpenSearchFilterCommand = new DelegateCommand(ShowSearchFilterDialog);
        }

        public IngridientManagementViewModel(Action<object, object[]> navigate, object[] parameters) : this()
        {
            this.navigate = navigate;
        }

        // getter and setter
        public ObservableCollection<IngridientModel> ListIngridient
        {
            get { return listIngridientModel.List; }
            set
            {
                listIngridientModel.List = value;
                //SelectedIngridient = ListIngridient[0];
                NotifyChanged("ListIngridient");
                NotifyProductChange();
            }
        }

        public IngridientModel SelectedIngridient { get; set; }

        public DelegateCommand<IngridientModel> IngridientSelectionChangeCommand { get; set; }
        public DelegateCommand<TextBox> SearchTextChangeCommand { get; set; }
        public DelegateCommand AddIngridientCommand { get; set; }
        public DelegateCommand UpdateIngridientCommand { get; set; }
        public DelegateCommand RemoveIngridientCommand { get; set; }
        public DelegateCommand OpenSearchFilterCommand { get; set; }

        private void IngridientChange(IngridientModel ingridient)
        {
            NotifyProductChange();
        }

        private void NotifyProductChange()
        {
            NotifyChanged("Name");
            NotifyChanged("Unit");
            NotifyChanged("Price");
            NotifyChanged("Image");
            NotifyChanged("MaxImportIngridient");
            NotifyChanged("ImportIngridient");
            NotifyChanged("Popular");
            NotifyChanged("SelectedIngridient");
        }

        public string Name
        {
            get { return SelectedIngridient.Name; }
        }

        public string Unit
        {
            get { return UnitModel.GetModelFromID(SelectedIngridient.UnitID).Name; }
        }

        public string Price
        {
            get { return SelectedIngridient.Price.ToString(); }
        }

        public string Image
        {
            get { return SelectedIngridient.ImageSource; }
        }

        public int MaxImportIngridient
        {
            get { return ListDetailImportationModel.GetSumDetailImportation(); }
        }

        public int ImportIngridient
        {
            get { return ListDetailImportationModel.GetSumDetailImportationFromIngridient(SelectedIngridient.ID); }
        }

        public string Popular
        {
            get
            {
                double p = Convert.ToDouble(ImportIngridient) / Convert.ToDouble(MaxImportIngridient);
                return (Math.Round(p, 2) * 100).ToString() + "%";
            }
        }

        private void ShowAddIngridientDialog()
        {
            (new AddIngridientDialog()).ShowDialog();

            //reload list ingridient
            listIngridientModel.LoadAllIngridient();
            bufferList = listIngridientModel.List;
        }

        private void OnSearchTextChange(TextBox textBox)
        {
            if (textBox.Text.Length == 0)
            {
                listIngridientModel.LoadAllIngridient();
                NotifyProductChange();
                return;
            }

            IEnumerable<IngridientModel> listMatch = null;
            if (fieldSearch == "Name")
            {
                listMatch = from ingridient in bufferList
                            where ingridient.Name.ToLower().Contains(textBox.Text.ToLower())
                            select new IngridientModel(ingridient);
            }
            else if (fieldSearch == "Price" && textBox.Text.Length >= 3)
            {

                if (textBox.Text.Contains(">="))
                {
                    double compare = Convert.ToDouble(textBox.Text.Remove(0, 2));
                    listMatch = from ingridient in bufferList
                                where ingridient.Price >= compare
                                select new IngridientModel(ingridient);
                }
                else if (textBox.Text.Contains("<="))
                {
                    double compare = Convert.ToDouble(textBox.Text.Remove(0, 2));
                    listMatch = from ingridient in bufferList
                                where ingridient.Price <= compare
                                select new IngridientModel(ingridient);
                }
                else if (textBox.Text.Contains(">"))
                {
                    double compare = Convert.ToDouble(textBox.Text.Remove(0, 1));
                    listMatch = from ingridient in bufferList
                                where ingridient.Price > compare
                                select new IngridientModel(ingridient);
                }
                else if (textBox.Text.Contains("<"))
                {
                    double compare = Convert.ToDouble(textBox.Text.Remove(0, 1));
                    listMatch = from ingridient in bufferList
                                where ingridient.Price < compare
                                select new IngridientModel(ingridient);
                }
                else
                {
                    double compare;
                    if (textBox.Text.Contains("="))
                        compare = Convert.ToDouble(textBox.Text.Remove(0, 1));
                    else
                        compare = Convert.ToDouble(textBox.Text);

                    listMatch = from ingridient in bufferList
                                where ingridient.Price == compare
                                select new IngridientModel(ingridient);
                }
            }

            if (listMatch == null)
                return;

            if (sortSearch != null)
            {
                if (sortSearch == "ASC")
                {
                    listMatch = listMatch.OrderBy(ingridient => ingridient.GetType().GetProperty(fieldSearch));
                }
                if (sortSearch == "DESC")
                {
                    listMatch = listMatch.OrderByDescending(ingridient => ingridient.GetType().GetProperty(fieldSearch));
                }
            }

            ListIngridient.Clear();
            ListIngridient = new ObservableCollection<IngridientModel>(listMatch);
        }

        private void UpdateIngridient()
        {
            (new AddIngridientDialog(SelectedIngridient)).ShowDialog();
            listIngridientModel.LoadAllIngridient();
            bufferList = listIngridientModel.List;
        }

        private void RemoveIngridient()
        {
            ConfirmDialog confirmDialog =
                new ConfirmDialog("CẢNH BÁO", "Nguyên liệu này sẽ không tồn tại trong hệ thống nếu tiếp tục. Xác nhận xóa nguyên liệu?",
                 (Action)delegate
                 {
                     IngridientModel.RemoveFromDatabase(SelectedIngridient.ID);
                 });
            confirmDialog.ShowDialog();
            listIngridientModel.LoadAllIngridient();
            bufferList = listIngridientModel.List;
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
