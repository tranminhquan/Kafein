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
    public class ListProductViewModel: BaseViewModel
    {
        private ListProductModel listProductModel;
        public ListProductViewModel(): base()
        {
            listProductModel = new ListProductModel();

            ListProduct.Add(new ProductModel("1", "Cà phê", "Ly", "Unit", 15000, null));
            ListProduct.Add(new ProductModel("1", "Cà phê sữa", "Ly", "Unit", 2000, null));
            ListProduct.Add(new ProductModel("1", "Sinh tố", "Ly", "Unit", 35000, null));
            ListProduct.Add(new ProductModel("1", "Sting", "Chai", "Unit", 12000, null));
            ListProduct.Add(new ProductModel("1", "Cà phê", "Ly", "Unit", 15000, null));
            ListProduct.Add(new ProductModel("1", "Cà phê", "Ly", "Unit", 15000, null));
        }

        // getter and setter
        public ObservableCollection<ProductModel> ListProduct
        {
            get { return listProductModel.List; }
            set { listProductModel.List = value; NotifyChanged("ListProduct"); }
        }
    }
}
