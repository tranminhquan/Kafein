using Kafein.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    class ListDetailBillModel: BaseList<DetailBillItemViewModel>
    {
        private static ListDetailBillModel instance = null;
        
        public ListDetailBillModel(): base()
        {

        }

        public static ListDetailBillModel GetInstance()
        {
            if (instance == null)
                instance = new ListDetailBillModel();
            return instance;
        }

        public ObservableCollection<dynamic> ListDetail
        {
            get { return this.GetCollectionOfField("DetailBillModel"); }
        }
    }
}
