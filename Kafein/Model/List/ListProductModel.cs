using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListProductModel: BaseList<ProductModel>
    {
        private static ListProductModel instance = null;
        public ListProductModel(): base()
        {

        }

        public static ListProductModel GetInstance()
        {
            if (instance == null)
                instance = new ListProductModel();
            return instance;
        }
    }
}
