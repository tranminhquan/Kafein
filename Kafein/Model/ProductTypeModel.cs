using Kafein.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public static ProductTypeModel GetModelFromID(string id)
        {
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM LOAIMATHANG WHERE MaLoaiMatHang='" + id + "'");
            while (reader.Read())
            {
                ProductTypeModel type = new ProductTypeModel(reader.GetString(0), reader.GetString(1));
                sqldb.Close();
                return type;
            }
            sqldb.Close();
            return null;
        }
    }
}
