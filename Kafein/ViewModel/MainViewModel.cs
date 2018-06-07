using System.ComponentModel;

namespace Kafein.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private object selectedViewModel;
        public MainViewModel()
        {
            SelectedViewModel = new BilllManagementViewModel(ViewModelNavigator, null);
            //SelectedViewModel = new ProductManagementViewModel(ViewModelNavigator, null);
        }

        // getter and setter
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; NotifyChanged("SelectedViewModel"); }
        }

        public void ViewModelNavigator(object obj, object[] parameters)
        {
            if (obj.ToString() == "BillManagementViewModel")
                SelectedViewModel = new BilllManagementViewModel(ViewModelNavigator, parameters);
            if (obj.ToString() == "ListProductViewModel")
                SelectedViewModel = new ListProductViewModel(ViewModelNavigator, parameters);
            if (obj.ToString() == "ProductManagementViewModel")
                SelectedViewModel = new ProductManagementViewModel(ViewModelNavigator, parameters);
        }
    }
}