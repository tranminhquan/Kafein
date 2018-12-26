using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.ViewModel
{
    public class ImportationManagementViewModel: BaseViewModel
    {
        public ImportationManagementViewModel(): base()
        {

        }

        public ImportationManagementViewModel(Action<object, object[]> navigate, object[] parameters) : this()
        {
            this.navigate = navigate;
        }


    }
}
