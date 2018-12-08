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
    public class ListIngridientModel : BaseList<IngridientModel>
    {
        private static ListIngridientModel instance = null;
        public ListIngridientModel() : base()
        {

        }

        public static ListIngridientModel GetInstance()
        {
            if (instance == null)
                instance = new ListIngridientModel();
            return instance;
        }

        public ObservableCollection<object> ListName
        {
            get { return this.GetCollectionOfField("Name"); }
        }

        public ObservableCollection<object> ListUnitID
        {
            get { return this.GetCollectionOfField("UnitID"); }
        }

        public ObservableCollection<object> ListPrice
        {
            get { return this.GetCollectionOfField("Price"); }
        }

        public void LoadAllIngridient()
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM NGUYENLIEU");
            while (reader.Read())
            {
                try
                {
                    if (!reader.IsDBNull(6))
                        continue;

                    string id = reader.GetString(0);
                    string name = reader.GetString(1);
                    string unitid = reader.GetString(2);
                    double price = reader.GetSqlMoney(3).ToDouble();
                    string image;
                    if (reader.IsDBNull(5))
                        image = null;
                    else
                    {
                        image = reader.GetString(5);
                        Debug.LogOutput(image);
                    }

                    IngridientModel ingridient = new IngridientModel(id, name, unitid, price, image);
                    this.List.Add(ingridient);
                }
                catch (SqlException)
                {

                }
            }
        }
    }
}
