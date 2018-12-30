using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class ExpenditureModel
    {
        public string ImportationDetaillID { get; set; }
        public string ImportationID { get; set; }
        public string IngredientName { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public double Value { get; set; }
        public double Price { get; set; }

        public ExpenditureModel()
        {

        }

        public ExpenditureModel(string importationdetailid, string importationid, string ingredientname, DateTime date, int quantity, double value, double price)
        {
            ImportationDetaillID = importationdetailid;
            ImportationID = importationid;
            IngredientName = ingredientname;
            Date = date;
            Quantity = quantity;
            Value = value;
            Price = price;
        }
    }
}
