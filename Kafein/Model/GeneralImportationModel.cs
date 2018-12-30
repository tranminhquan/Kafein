using Kafein.Model.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kafein.Model.Importation;

namespace Kafein.Model
{
    public class GeneralImportationModel
    {
        public ImportationModel Importation { get; set; }
        public ListDetailImportationModel ListDetailImportation { get; set; }

        public GeneralImportationModel()
        {
            Importation = new ImportationModel();
            ListDetailImportation = new ListDetailImportationModel();
        }

        public GeneralImportationModel(ImportationModel importation, ListDetailImportationModel list)
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
