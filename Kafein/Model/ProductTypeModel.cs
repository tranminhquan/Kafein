using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class ProductTypeModel
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public ProductTypeModel()
        {

        }

        public ProductTypeModel(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
