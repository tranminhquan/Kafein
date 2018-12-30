using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListGeneralImportationModel: BaseList<GeneralImportationModel>, INotifyPropertyChanged
    {
        private static ListGeneralImportationModel instance;

        public ListGeneralImportationModel(): base()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static ListGeneralImportationModel GetInstance()
        {
            if (instance == null)
                instance = new ListGeneralImportationModel();
            return instance;
        }

        public void NotifyListChange()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("List"));
        }
       

    }
}
