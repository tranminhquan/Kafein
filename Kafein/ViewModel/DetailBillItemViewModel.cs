using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kafein.Model;
using Kafein.Model.List;
using Kafein.Utilities;
using Prism.Commands;

namespace Kafein.ViewModel
{
    // represent an item of list detail bill model (contend remove buttom command, product name, unit, quantity, price)
    // need to include Product model, Unit model, DetailBill model
    public class DetailBillItemViewModel: BaseViewModel
    {
        private ProductModel productModel;
        private UnitModel unitModel;
        private DetailBillModel detailBillModel;


        public DetailBillItemViewModel(): base()
        {
            productModel = new ProductModel();
            unitModel = new UnitModel();
            detailBillModel = new DetailBillModel();
            RemoveItemCommand = new DelegateCommand<DetailBillItemViewModel>(RemoveItem);
        }

        public DetailBillItemViewModel(ProductModel product, UnitModel unit, DetailBillModel detail)
        {
            productModel = product;
            unitModel = unit;
            detailBillModel = detail;
        }

        // getter and setter need for detail bill view
        public DetailBillModel DetailBillModel
        {
            get { return detailBillModel; }
            set { detailBillModel = value; NotifyChanged("DetailBillModel"); }
        }
        public string ProductName
        {
            get { return productModel.Name; }
            set { productModel.Name = value; NotifyChanged("ProductName"); }
        }


        public string UnitName
        {
            get { return unitModel.Name; }
            set { unitModel.Name = value; NotifyChanged("UnitName"); }
        }

        public int Quantity
        {
            get { return detailBillModel.Quantity; }
            set { detailBillModel.Quantity = value; NotifyChanged("Quantity"); NotifyChanged("Price"); }
        }

        public double Price
        {
            get { return detailBillModel.Price * detailBillModel.Quantity; }
        }

        public DelegateCommand<DetailBillItemViewModel> RemoveItemCommand { get; set; }

        private void RemoveItem(DetailBillItemViewModel item)
        {
            Debug.LogOutput("Remove item command " + item.ProductName);
        }
    }
}
