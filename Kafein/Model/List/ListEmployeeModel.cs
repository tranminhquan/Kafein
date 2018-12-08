using Kafein.Database;
using Kafein.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListEmployeeModel : BaseList<EmployeeModel>
    {
        private static ListEmployeeModel instance = null;
        public ListEmployeeModel() : base()
        {

        }

        public static ListEmployeeModel GetInstance()
        {
            if (instance == null)
                instance = new ListEmployeeModel();
            return instance;
        }
    }
}
