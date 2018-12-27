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
    // represent an item of list detail importation model (contend remove buttom command, ingridient name, unit, quantity, price)
    // need to include Ingridient model, Unit model, DetailImportation model
    public class DetailImportationItemViewModel: BaseViewModel
    {
        private IngridientModel ingridientModel;
        private UnitModel unitModel;
        private DetailImportationModel detailImportationModel;


        public DetailImportationItemViewModel(): base()
        {
            ingridientModel = new IngridientModel();
            unitModel = new UnitModel();
            detailImportationModel = new DetailImportationModel();
            RemoveItemCommand = new DelegateCommand<DetailImportationItemViewModel>(RemoveItem);
        }

        public DetailImportationItemViewModel(IngridientModel ingridient, UnitModel unit, DetailImportationModel detail)
        {
            ingridientModel = ingridient;
            unitModel = unit;
            detailImportationModel = detail;
        }

        // getter and setter need for detail bill view
        public DetailImportationModel DetailImportationModel
        {
            get { return detailImportationModel; }
            set { detailImportationModel = value; NotifyChanged("DetailImportationModel"); }
        }
        public string IngridientName
        {
            get { return ingridientModel.Name; }
            set { ingridientModel.Name = value; NotifyChanged("IngridientName"); }
        }


        public string UnitName
        {
            get { return unitModel.Name; }
            set { unitModel.Name = value; NotifyChanged("UnitName"); }
        }

        public int Quantity
        {
            get { return detailImportationModel.Quantity; }
            set { detailImportationModel.Quantity = value; NotifyChanged("Quantity"); NotifyChanged("Price"); }
        }

        public double Price
        {
            get { return detailImportationModel.Price * detailImportationModel.Quantity; }
        }

        public DelegateCommand<DetailImportationItemViewModel> RemoveItemCommand { get; set; }

        private void RemoveItem(DetailImportationItemViewModel item)
        {
            Debug.LogOutput("Remove item command " + item.IngridientName);
        }
    }
}
