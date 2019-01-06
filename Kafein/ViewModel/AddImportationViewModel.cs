using Kafein.Model;
using Kafein.Model.Importation;
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
using System.Windows;
using System.Windows.Input;

namespace Kafein.ViewModel
{
    public class AddImportationViewModel: BaseViewModel
    {
        private ListDetailImportationModel listDetailImportation;
        private ImportationModel newImportation;
        private ListExpenditureModel listExpenditureModel;

        public AddImportationViewModel(): base()
        {
            listExpenditureModel = ListExpenditureModel.GetInstance();
            listDetailImportation = new ListDetailImportationModel();
            IngridientSelectionChangeCommand = new DelegateCommand<IngridientModel>(IngridientChange);
            CreateImportationCommand = new DelegateCommand(CreateImportation);
            ClearImportationCommand = new DelegateCommand(ClearImportation);

            newImportation = new ImportationModel();
            newImportation.ID = ImportationModel.GenerateID(ListImportationModel.GetInstance().List);

            SelectedIndex = 0;
            SelectedIndex = -1;
            NotifyChanged("SelectedIndex");
        }
        public double SumPrice
        {
            get
            {
                double sum = 0;
                foreach (DetailImportationItemViewModel item in ListDetailImportation)
                {
                    sum += item.Price;
                }
                return sum;
            }
        }
        public string Date
        {
            get { return DateTime.Now.ToShortDateString(); }
        }

        public AddImportationViewModel(Action<object, object[]> navigate, object[] parameters) : this()
        {
            this.navigate = navigate;
        }

        public ObservableCollection<DetailImportationItemViewModel> ListDetailImportation
        {
            get { return listDetailImportation.List; }
            set { listDetailImportation.List = value; NotifyChanged("ListDetailImportation"); }
        }

        public int SelectedIndex { get; set; }
        public DelegateCommand<IngridientModel> IngridientSelectionChangeCommand { get; set; }
        public DelegateCommand CreateImportationCommand { get; set; }
        public DelegateCommand ClearImportationCommand { get; set; }

        public void IngridientChange(IngridientModel ingridient)
        {        
            if (SelectedIndex == -1)
                return;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                foreach (DetailImportationItemViewModel item in ListDetailImportation)
                {
                    if (item.IngridientName.Equals(ingridient.Name))
                    {
                        item.Quantity++;
                        NotifyDetailImportationProperty();
                        return;
                    }
                }

                UnitModel unit = UnitModel.GetModelFromID(ingridient.UnitID);

                DetailImportationModel detail = new DetailImportationModel(DetailImportationModel.GenerateID(listDetailImportation.ListDetail), newImportation.ID, ingridient.ID, unit.ID, 1, ingridient.Price);
                listDetailImportation.Add(new DetailImportationItemViewModel(ingridient, unit, detail));

                NotifyDetailImportationProperty();
            }
        }

        public void NotifyDetailImportationProperty()
        {
            SelectedIndex = -1;
            NotifyChanged("SelectedIndex");
            NotifyChanged("SumPrice");
        }

        public void CreateImportation()
        {
            InitImportation();
            (new ConfirmDialog("XÁC NHẬN", "Thêm phiếu nhập hàng?", (Action)delegate
            {
                ImportationModel.SaveToDatabase(newImportation);
                foreach(DetailImportationItemViewModel item in ListDetailImportation)
                {
                    DetailImportationModel.SaveToDatabase(item.DetailImportationModel);
                }
            })).ShowDialog();
            listExpenditureModel.List.Clear();
            listExpenditureModel.LoadAllExpenditure();
        }

        public void InitImportation()
        {
            if (ListDetailImportation.Count == 0)
            {
                MessageBox.Show("Phiếu nhập rỗng", "Nhắc nhở", MessageBoxButton.OK);
                return;
            }

            newImportation.Date = DateTime.Now;
            newImportation.Price = SumPrice;
        }

        public void ClearImportation()
        {
            ListDetailImportation.Clear();
            NotifyChanged("SumPrice");
        }    
    }
}
