using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class ProductModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string UnitID { get; set; }
        public double Price { get; set; }

        public ProductModel()
        {

        }

        public ProductModel(string id, string name, string unitid, double price)
        {
            ID = id;
            Name = name;
            UnitID = unitid;
            Price = price;
        }
    }
}
