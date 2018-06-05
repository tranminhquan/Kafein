using Kafein.Database;
using Kafein.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListProductModel: BaseList<ProductModel>
    {
        private static ListProductModel instance = null;
        public ListProductModel(): base()
        {

        }

        public static ListProductModel GetInstance()
        {
            if (instance == null)
                instance = new ListProductModel();
            return instance;
        }

        public void LoadAllProduct()
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM MATHANG");
            while(reader.Read())
            {
                try
                {
                    string id = reader.GetString(0);
                    string name = reader.GetString(1);
                    string typeid = reader.GetString(2);
                    string unitid = reader.GetString(3);
                    double price = reader.GetSqlMoney(4).ToDouble();
                    string image;
                    if (reader.IsDBNull(5))
                        image = Environment.CurrentDirectory + "drink_default.png";
                    else
                    {
                        image = reader.GetString(5);
                        Debug.LogOutput(image);
                    }

                    ProductModel product = new ProductModel(id, name, typeid, unitid, price, image);
                    this.List.Add(product);
                }
                catch (SqlException)
                {

                }
            }
        }

        public void LoadProductFromType(string type)
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM MATHANG WHERE MaLoaiMatHang = '" + type + "'");
            while (reader.Read())
            {
                try
                {
                    string id = reader.GetString(0);
                    string name = reader.GetString(1);
                    string typeid = reader.GetString(2);
                    string unitid = reader.GetString(3);
                    double price = reader.GetSqlMoney(4).ToDouble();
                    string image;
                    if (reader.IsDBNull(5))
                        image = Environment.CurrentDirectory + "drink_default.png";
                    else
                    {
                        image = reader.GetString(5);
                        Debug.LogOutput(image);
                    }

                    ProductModel product = new ProductModel(id, name, typeid, unitid, price, image);
                    this.List.Add(product);
                }
                catch (SqlException)
                {

                }
            }
        }
    }
}
