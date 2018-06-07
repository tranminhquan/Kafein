using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListGeneralBillModel: BaseList<GeneralBillModel>, INotifyPropertyChanged
    {
        private static ListGeneralBillModel instance;

        public ListGeneralBillModel(): base()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static ListGeneralBillModel GetInstance()
        {
            if (instance == null)
                instance = new ListGeneralBillModel();
            return instance;
        }

        public void NotifyListChange()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("List"));
        }
       

    }
}
