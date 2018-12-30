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
    class DetailImportationViewModel : BaseViewModel
    {
        private ListDetailImportationModel listDetailImportation;

        public DetailImportationViewModel(): base()
        {
            // list item in detail importation
            //listDetailImportation = ListDetailImportationModel.GetInstance();
            listDetailImportation = new ListDetailImportationModel();
            IngridientSelectionChangeCommand = new DelegateCommand<IngridientModel>(SelectedIngridientChange);

            // =============> !!!! [WARNING] DO NOT DELETE THIS CODE !!!! <==============
            SelectedIndex = 0;
            SelectedIndex = -1;
            NotifyChanged("SelectedIndex");
            // ==================================> <======================================
        }


        // getter and setter
        public int SelectedIndex { get; set; }
        public DelegateCommand<IngridientModel> IngridientSelectionChangeCommand { get; set; }
        public ObservableCollection<DetailImportationItemViewModel> ListDetailImportation
        {
            get { return listDetailImportation.List; }
            set { listDetailImportation.List = value; NotifyChanged("ListDetailImportation"); }
        }

        public double SumPrice
        {
            get
            {
                double sum = 0;
                foreach (DetailImportationItemViewModel item in ListDetailImportation)
                {
                    sum += item.Price;
                }
                return sum;
            }
        }

        private void SelectedIngridientChange(IngridientModel ingridient)
        {
            if (SelectedIndex == -1)
                return;

            Debug.LogOutput(ingridient.Name);

            //Check if ingridient was chosen, then update quantity
            foreach (DetailImportationItemViewModel item in ListDetailImportation)
            {
                if (item.IngridientName.Equals(ingridient.Name))
                {
                    //update quantity
                    item.Quantity++;
                    NotifyDetaillImportationProperty();
                    return;
                }
            }

            //Otherwise, create the importation
            // model related
            UnitModel unit = UnitModel.GetModelFromID(ingridient.UnitID);

            // Generate id for detaill importation
            //DetailImportationModel detail = new DetailImportationModel(DetailImportationModel.GenerateID(listDetailImportation.ListDetail), newImportation.ID, ingridient.ID, unit.ID, 1, ingridient.Price);
            //listDetailImportation.Add(new DetailImportationItemViewModel(ingridient, unit, detail));

            NotifyDetaillImportationProperty();
        }

        private void NotifyDetaillImportationProperty()
        {
            throw new NotImplementedException();
        }
    }
}
