using Kafein.Model.List;
using Kafein.Model.SalesNPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class GeneralImportationModel
    {
        public ImportationModel Importation { get; set; }
        public ListDetailBillModel ListDetailImportation { get; set; }

        public GeneralImportationModel()
        {
            Importation = new ImportationModel();
            ListDetailImportation = new ListDetailBillModel();
        }

        public GeneralImportationModel(ImportationModel importation, ListDetailBillModel list)
        {
            this.Importation = importation;
            this.ListDetailImportation = list;
        }

        // getter and setter
        public string ImportTime
        {
            get { return Importation.ImportTime; }
        }
        public double Price
        {
            get { return Importation.Price; }
        }
    }
}
