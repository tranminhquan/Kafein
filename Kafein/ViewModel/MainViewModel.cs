using Kafein.Utilities;
using System.ComponentModel;

namespace Kafein.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private object selectedViewModel;
        public MainViewModel()
        {
            SelectedViewModel = new BillManagementViewModel(ViewModelNavigator, null);
            //SelectedViewModel = new ImportationManagementViewModel(ViewModelNavigator, null);
            //SelectedViewModel = new ProductManagementViewModel(ViewModelNavigator, null);
        }

        // getter and setter
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                selectedViewModel = value;
                NotifyChanged("SelectedViewModel");
                Debug.LogOutput(value.ToString());
            }
        }

        public void ViewModelNavigator(object obj, object[] parameters)
        {
            if (obj.ToString() == "BillManagementViewModel")
                SelectedViewModel = new BillManagementViewModel(ViewModelNavigator, parameters);
            if (obj.ToString() == "ListProductViewModel")
                SelectedViewModel = new ListProductViewModel(ViewModelNavigator, parameters);
            if (obj.ToString() == "ProductManagementViewModel")
                SelectedViewModel = new ProductManagementViewModel(ViewModelNavigator, parameters);
            if (obj.ToString() == "ImportationManagementViewModel")
                SelectedViewModel = new ImportationManagementViewModel(ViewModelNavigator, parameters);
            if (obj.ToString() == "AddImportationViewModel")
                SelectedViewModel = new AddImportationViewModel(ViewModelNavigator, parameters);
            if (obj.ToString() == "ListIngridientViewModel")
                SelectedViewModel = new ListIngridientViewModel(ViewModelNavigator, parameters);
            if (obj.ToString() == "IngridientManagementViewModel")
                SelectedViewModel = new IngridientManagementViewModel(ViewModelNavigator, parameters);
            if (obj.ToString() == "ReportManagementViewModel")
                SelectedViewModel = new ReportManagementViewModel(ViewModelNavigator, parameters);
        }
    }
}