using Kafein.Model;
using Kafein.Model.List;
using Kafein.Utilities;
using Prism.Commands;
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
        private ObservableCollection<DetailBillItemViewModel> listDetailBill;
        public ListProductViewModel(): base()
        {
            listProductModel = new ListProductModel();
            //test
            ListProduct.Add(new ProductModel("1", "Cà phê", "Ly", "Unit", 15000, null));
            ListProduct.Add(new ProductModel("1", "Cà phê sữa", "Ly", "Unit", 2000, null));
            ListProduct.Add(new ProductModel("1", "Sinh tố", "Ly", "Unit", 35000, null));
            ListProduct.Add(new ProductModel("1", "Sting", "Chai", "Unit", 12000, null));
            ListProduct.Add(new ProductModel("1", "Cà phê", "Ly", "Unit", 15000, null));
            ListProduct.Add(new ProductModel("1", "Cà phê", "Ly", "Unit", 15000, null));

            listDetailBill = new ObservableCollection<DetailBillItemViewModel>();
            //test
            ListDetailBill.Add(new DetailBillItemViewModel("Cà phê đá", "Ly", 2, 30000));
            ListDetailBill.Add(new DetailBillItemViewModel("Cà phê sữa đá", "Ly", 1, 20000));
            ListDetailBill.Add(new DetailBillItemViewModel("Sinh tố bơ", "Ly", 1, 30000));
            ListDetailBill.Add(new DetailBillItemViewModel("Sinh tố dâu", "Ly", 1, 25000));
            ListDetailBill.Add(new DetailBillItemViewModel("Sting", "Chai", 2, 20000));

            // command init
            ProductSelectionChangeCommand = new DelegateCommand<ProductModel>(SelectedProductChange);
        }

        // getter and setter
        public DelegateCommand<ProductModel> ProductSelectionChangeCommand { get; set; }
        public ObservableCollection<DetailBillItemViewModel> ListDetailBill
        {
            get { return listDetailBill; }
            set { listDetailBill = value; NotifyChanged("ListDeta"); }
        }
        public ObservableCollection<ProductModel> ListProduct
        {
            get { return listProductModel.List; }
            set { listProductModel.List = value; NotifyChanged("ListProduct"); }
        }

        private void SelectedProductChange(ProductModel product)
        {
            Debug.LogOutput(product.Name);

            // model related
            UnitModel unit = UnitModel.GetModelFromID(product.UnitID);
           
            // Generate id for detaill bill
            //DetailBillModel detail = new DetailBillModel(UnitModel.GenerateID(), product.ID, unit.ID, )
        }
    }
}
