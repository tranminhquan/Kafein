using GalaSoft.MvvmLight;
using System.ComponentModel;

namespace Kafein.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private object selectedViewModel;
        public MainViewModel()
        {
            SelectedViewModel = new BilllManagementViewModel(ViewModelNavigator);
        }

        // getter and setter
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; NotifyChanged("SelectedViewModel"); }
        }

        public void ViewModelNavigator(object obj)
        {
            if (obj.ToString() == "BillManagementViewModel")
                SelectedViewModel = new BilllManagementViewModel(ViewModelNavigator);
            //if (obj.ToString() == "ListProductViewModel")
            //    SelectedViewModel = new ListProductViewModel(ViewModelNavigator);
        }
    }
}