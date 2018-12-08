using Kafein.Model.SalesNPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListImportationModel : BaseList<ImportationModel>
    {
        private static ListImportationModel instance = null;
        public ListImportationModel() : base()
        {

        }

        public static ListImportationModel GetInstace()
        {
            if (instance == null)
                instance = new ListImportationModel();
            return instance;
        }
    }
}
