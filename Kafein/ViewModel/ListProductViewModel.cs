using Kafein.Model;
using Kafein.Model.List;
using Kafein.Model.SalesNPay;
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
        private BillModel newBill;
        private ObservableCollection<DetailBillItemViewModel> listDetailBill;
        public ListProductViewModel(): base()
        {
            listProductModel = ListProductModel.GetInstance();
            //test
            ListProduct.Add(new ProductModel("MH001", "Cà phê", "LMH1", "DVT1", 15000, null));
            ListProduct.Add(new ProductModel("MH002", "Cà phê sữa", "LMH1", "DVT1", 2000, null));
            ListProduct.Add(new ProductModel("MH003", "Sinh tố bơ", "LMH2", "DVT1", 35000, null));
            ListProduct.Add(new ProductModel("MH004", "Sting", "LMH3", "DVT3", 12000, null));
            ListProduct.Add(new ProductModel("MH005", "Redbull", "LMH3", "DVT2", 15000, null));
            ListProduct.Add(new ProductModel("MH006", "Pepsi", "LMH3", "DVT2", 8000, null));

            listDetailBill = new ObservableCollection<DetailBillItemViewModel>();
            //test
            //ListDetailBill.Add(new DetailBillItemViewModel("Cà phê đá", "Ly", 2, 30000));
            //ListDetailBill.Add(new DetailBillItemViewModel("Cà phê sữa đá", "Ly", 1, 20000));
            //ListDetailBill.Add(new DetailBillItemViewModel("Sinh tố bơ", "Ly", 1, 30000));
            //ListDetailBill.Add(new DetailBillItemViewModel("Sinh tố dâu", "Ly", 1, 25000));
            //ListDetailBill.Add(new DetailBillItemViewModel("Sting", "Chai", 2, 20000));

            // command init
            ProductSelectionChangeCommand = new DelegateCommand<ProductModel>(SelectedProductChange);


            // =============> !!!! [WARNING] DO NOT DELETE THIS CODE !!!! <==============
            SelectedIndex = 0;
            SelectedIndex = -1;
            NotifyChanged("SelectedIndex");
            // ==================================> <======================================
        }

        public ListProductViewModel(Action<object, object[]> navigate, object[] parameters): this()
        {
            this.navigate = navigate;

            if (parameters == null)
            {
                newBill = new BillModel();
                newBill.ID = BillModel.GenerateID();
            }
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

        public int SelectedIndex { get; set; }

        public double SumPrice
        {
            get
            {
                double sum = 0;
                foreach (DetailBillItemViewModel item in ListDetailBill)
                {
                    sum += (item.Quantity * item.Price);
                }
                return sum;
            }
        }

        private void SelectedProductChange(ProductModel product)
        {
            if (SelectedIndex == -1)
                return;

            Debug.LogOutput(product.Name);

            //Check if product was chosen, then update quantity
            foreach(DetailBillItemViewModel item in listDetailBill)
            {
                if (item.ProductName.Equals(product.Name))
                {
                    //update quantity
                    item.Quantity++;
                    NotifyDetaillBillProperty();
                    return;
                }
            }

            //Otherwise, create the bill
            // model related
            UnitModel unit = UnitModel.GetModelFromID(product.UnitID);

            // Generate id for detaill bill
            DetailBillModel detail = new DetailBillModel(DetailBillModel.GenerateID(), newBill.ID, product.ID, unit.ID, 1, product.Price);
            listDetailBill.Add(new DetailBillItemViewModel(product, unit, detail));

            NotifyDetaillBillProperty();
        }

        private void NotifyDetaillBillProperty()
        {
            SelectedIndex = -1;
            NotifyChanged("SelectedIndex");
            NotifyChanged("SumPrice");
        }
    }
}
