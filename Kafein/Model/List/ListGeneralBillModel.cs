using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListGeneralBillModel: BaseList<GeneralBillModel>
    {
        private static ListGeneralBillModel instance;

        public ListGeneralBillModel(): base()
        {

        }

        public static ListGeneralBillModel GetInstance()
        {
            if (instance == null)
                instance = new ListGeneralBillModel();
            return instance;
        }

       

    }
}
