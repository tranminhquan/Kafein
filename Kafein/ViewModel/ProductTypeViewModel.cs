using Kafein.Model;
using Kafein.Model.List;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.ViewModel
{
    public class ProductTypeViewModel: BaseViewModel
    {
        private ListProductTypeModel listProductTypeModel;

        public ProductTypeViewModel()
        {
            listProductTypeModel = new ListProductTypeModel();

            //test
            ListProductType.Add(new ProductTypeModel("1", "Nước giải khát"));
            ListProductType.Add(new ProductTypeModel("2", "Cà phê"));
            ListProductType.Add(new ProductTypeModel("3", "Sinh tố"));
        }

        //public ObservableCollection<string> ListTypeName
        //{
        //    get
        //    {
        //        //IEnumerable<string> obsCollection = (IEnumerable<string>)listProductTypeModel.ListName;
        //        //return new ObservableCollection<string>(obsCollection);
        //        return new ObservableCollection<string>() { "1", "2", "3" };
        //    }
        //}

        public ObservableCollection<ProductTypeModel> ListProductType
        {
            get { return listProductTypeModel.List; }
            set { listProductTypeModel.List = value; NotifyChanged("ListProductType"); }
        }
    }
}
