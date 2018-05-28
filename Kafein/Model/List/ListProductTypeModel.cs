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
    class ListProductTypeModel: BaseList<ProductTypeModel>
    {
        public ListProductTypeModel(): base()
        {

        }

        public ObservableCollection<object> ListName
        {
            get { return this.GetCollectionOfField("Name"); }
        }

        public void LoadAllProductType()
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM LOAIMATHANG");
            while(reader.Read())
            {
                try
                {
                    this.List.Add(new ProductTypeModel(reader.GetString(0), reader.GetString(1)));
                }
                catch (SqlException)
                {

                }
            }
            sqldb.Close();
        }
    }
}
