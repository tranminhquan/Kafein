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
        private ObservableCollection<DetailBillItemViewModel> listDetailBill;

        public DetailBillViewModel(): base()
        {
            // list item in detail bill
            listDetailBill = new ObservableCollection<DetailBillItemViewModel>();
            //test
            ListDetailBill.Add(new DetailBillItemViewModel("Cà phê đá", "Ly", 2, 30000));
            ListDetailBill.Add(new DetailBillItemViewModel("Cà phê sữa đá", "Ly", 1, 20000));
            ListDetailBill.Add(new DetailBillItemViewModel("Sinh tố bơ", "Ly", 1, 30000));
            ListDetailBill.Add(new DetailBillItemViewModel("Sinh tố dâu", "Ly", 1, 25000));
            ListDetailBill.Add(new DetailBillItemViewModel("Sting", "Chai", 2, 20000));

            
        }


        // getter and setter
        public ObservableCollection<DetailBillItemViewModel> ListDetailBill
        {
            get { return listDetailBill; }
            set { listDetailBill = value; NotifyChanged("ListDeta"); }
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
    }
}
