using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kafein.Model;
using Kafein.Model.List;
using Kafein.Utilities;
using Prism.Commands;

namespace Kafein.ViewModel
{
    class DetailBillViewModel: BaseViewModel
    {
        private ListDetailBillModel listDetailBill;

        public DetailBillViewModel(): base()
        {
            // list item in detail bill
            //listDetailBill = ListDetailBillModel.GetInstance();
            listDetailBill = new ListDetailBillModel();
            ProductSelectionChangeCommand = new DelegateCommand<ProductModel>(SelectedProductChange);

            // =============> !!!! [WARNING] DO NOT DELETE THIS CODE !!!! <==============
            SelectedIndex = 0;
            SelectedIndex = -1;
            NotifyChanged("SelectedIndex");
            // ==================================> <======================================
        }


        // getter and setter
        public int SelectedIndex { get; set; }
        public DelegateCommand<ProductModel> ProductSelectionChangeCommand { get; set; }
        public ObservableCollection<DetailBillItemViewModel> ListDetailBill
        {
            get { return listDetailBill.List; }
            set { listDetailBill.List = value; NotifyChanged("ListDetailBill"); }
        }

        public double SumPrice
        {
            get
            {
                double sum = 0;
                foreach (DetailBillItemViewModel item in ListDetailBill)
                {
                    sum += item.Price;
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
            foreach (DetailBillItemViewModel item in ListDetailBill)
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
            //DetailBillModel detail = new DetailBillModel(DetailBillModel.GenerateID(listDetailBill.ListDetail), newBill.ID, product.ID, unit.ID, 1, product.Price);
            //listDetailBill.Add(new DetailBillItemViewModel(product, unit, detail));

            NotifyDetaillBillProperty();
        }

        private void NotifyDetaillBillProperty()
        {
            throw new NotImplementedException();
        }
    }
}
