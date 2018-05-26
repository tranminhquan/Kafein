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
        public ListDetailBillModel(): base()
        {

        }

        public ObservableCollection<dynamic> ListDetail
        {
            get { return this.GetCollectionOfField("DetailBillModel"); }
        }
    }
}
