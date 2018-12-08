using Kafein.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class PositionModel
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public PositionModel()
        {

        }

        public PositionModel(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public static PositionModel GetModelFromID(string id)
        {
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM CHUCVU WHERE MaChucVu='" + id + "'");
            while (reader.Read())
            {
                PositionModel type = new PositionModel(reader.GetString(0), reader.GetString(1));
                sqldb.Close();
                return type;
            }
            sqldb.Close();
            return null;
        }
    }
}
