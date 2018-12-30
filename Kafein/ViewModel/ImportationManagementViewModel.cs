using Kafein.Model;
using Kafein.Model.List;
using Kafein.Model.Importation;
using Kafein.Utilities;
using Kafein.View.Dialog;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.ViewModel
{
    public class ImportationManagementViewModel : BaseViewModel
    {
        private ListGeneralImportationModel listGeneralImportationModel;

        public ImportationManagementViewModel() : base()
        {
            listGeneralImportationModel = ListGeneralImportationModel.GetInstance();

            ////test
            //ListImportation.Add(new ImportationModel("1", DateTime.Now, 125000));
            //ListImportation.Add(new ImportationModel("2", DateTime.Now, 5000));
            //ListImportation.Add(new ImportationModel("3", DateTime.Now, 75000));
            //ListImportation.Add(new ImportationModel("4", DateTime.Now, 25000));
            //ListImportation.Add(new ImportationModel("4", DateTime.Now, 25000));
            //ListImportation.Add(new ImportationModel("4", DateTime.Now, 25000));
            //ListImportation.Add(new ImportationModel("4", DateTime.Now, 25000));
            //ListImportation.Add(new ImportationModel("4", DateTime.Now, 25000));
            //ListImportation.Add(new ImportationModel("4", DateTime.Now, 25000));
            //ListImportation.Add(new ImportationModel("4", DateTime.Now, 25000));

            //listGeneralImportationModel.Add(new GeneralImportationModel(new ImportationModel("1", DateTime.Now, 125000), new ListDetailImportationModel()));
            //listGeneralImportationModel.Add(new GeneralImportationModel(new ImportationModel("2", DateTime.Now, 12000), new ListDetailImportationModel()));
            //listGeneralImportationModel.Add(new GeneralImportationModel(new ImportationModel("3", DateTime.Now, 25000), new ListDetailImportationModel()));
            //listGeneralImportationModel.Add(new GeneralImportationModel(new ImportationModel("4", DateTime.Now, 15000), new ListDetailImportationModel()));
            //listGeneralImportationModel.Add(new GeneralImportationModel(new ImportationModel("5", DateTime.Now, 5000), new ListDetailImportationModel()));
            //listGeneralImportationModel.Add(new GeneralImportationModel(new ImportationModel("6", DateTime.Now, 15000), new ListDetailImportationModel()));
            //listGeneralImportationModel.Add(new GeneralImportationModel(new ImportationModel("7", DateTime.Now, 25000), new ListDetailImportationModel()));


            CreateImportationCommand = new DelegateCommand(CreateImportation);
            DetailCommand = new DelegateCommand(ShowDetail);
            CheckoutCommand = new DelegateCommand(ShowCheckoutDialog);
        }

        public ImportationManagementViewModel(Action<object, object[]> navigate, object[] parameters) : this()
        {
            this.navigate = navigate;
        }

        // getter and setter
        public ObservableCollection<GeneralImportationModel> ListImportation
        {
            get { return listGeneralImportationModel.List; }
            set { listGeneralImportationModel.List = value; NotifyChanged("ListImportation"); }
        }

        public int SelectedIndexImportation { get; set; }


        public DelegateCommand CreateImportationCommand { get; set; }
        public DelegateCommand DetailCommand { get; set; }
        public DelegateCommand CheckoutCommand { get; set; }


        private void CreateImportation()
        {
            navigate.Invoke("ListIngridientViewModel", null);
        }

        private void ShowDetail()
        {
            navigate.Invoke("ListIngridientViewModel", new object[] { SelectedIndexImportation });
        }

        private void ShowCheckoutDialog()
        {
            //(new CheckoutDialog(navigate, listGeneralImportationModel.List[SelectedIndexImportation].Importation, listGeneralImportationModel.List[SelectedIndexImportation].ListDetailImportation.List, SelectedIndexImportation)).ShowDialog();
        }
    }
}
