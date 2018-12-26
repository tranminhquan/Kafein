using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.ViewModel
{
    public class ReportManagementViewModel: BaseViewModel
    {
        public ReportManagementViewModel(): base()
        {

        }

        public ReportManagementViewModel(Action<object, object[]> navigate, object[] parameters) : this()
        {
            this.navigate = navigate;
        }
    }
}
