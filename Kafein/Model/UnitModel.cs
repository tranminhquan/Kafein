using Kafein.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class UnitModel
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public UnitModel()
        {

        }

        public UnitModel(string id, string name)
        {
            ID = id;
            Name = name;
        }


        // static method
        public static UnitModel GetModelFromID(string id)
        {
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM DONVITINH WHERE MaDonViTinh='" + id + "'");
            while(reader.Read())
            {
                UnitModel unit = new UnitModel(reader.GetString(0), reader.GetString(1));
                sqldb.Close();
                return unit;
            }
            sqldb.Close();
            return null;
        }

        //public static string GenerateID()
        //{
        //    IDatabase sqldb = new SQLDatabase();
        //    sqldb.Open();
        //    SqlDataReader reader = sqldb.ExcuteReader("SELECT Max(MaDonViTinh) FROM DONVITINH");
        //    while(reader.Read())
        //    {
        //        string currentID = reader.GetString(0);
        //        string prefix = currentID.Substring(0, 3);
        //        int no = Convert.ToInt16(currentID.Substring(2, 1));
        //        no++;
        //        return prefix + (no.ToString());
        //    }

        //    return "DVT1";
        //}
    }
}
