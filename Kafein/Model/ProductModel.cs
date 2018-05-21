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
        public string TypeID { get; set; }
        public string UnitID { get; set; }
        public double Price { get; set; }
        public string PicturePath { get; set; }

        public ProductModel()
        {

        }

        public ProductModel(string id, string name, string typeid, string unitid, double price, string picturepath)
        {
            ID = id;
            Name = name;
            TypeID = typeid;
            UnitID = unitid;
            Price = price;
            PicturePath = picturepath;
        }
    }
}
