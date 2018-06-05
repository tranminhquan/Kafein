using Kafein.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListUnitModel: BaseList<UnitModel>
    {
        public ListUnitModel(): base()
        {

        }

        public ObservableCollection<object> ListName
        {
            get { return this.GetCollectionOfField("Name"); }
        }

        public void LoadAllUnit()
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM DONVITINH");
            while (reader.Read())
            {
                try
                {
                    this.List.Add(new UnitModel(reader.GetString(0), reader.GetString(1)));
                }
                catch (SqlException)
                {

                }
            }
            sqldb.Close();
        }
    }
}
