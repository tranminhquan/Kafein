using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    class ListProductTypeModel: BaseList<ProductTypeModel>
    {
        public ListProductTypeModel(): base()
        {

        }

        public ObservableCollection<object> ListName
        {
            get { return this.GetCollectionOfField("Name"); }
        }
    }
}
