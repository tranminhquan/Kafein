using Kafein.Database;
using Kafein.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public string ImageSource { get; set; }

        public ProductModel()
        {

        }

        public ProductModel(ProductModel product)
        {
            ID = product.ID;
            Name = product.Name;
            TypeID = product.TypeID;
            UnitID = product.UnitID;
            Price = product.Price;
            ImageSource = product.ImageSource;
        }

        public ProductModel(string id, string name, string typeid, string unitid, double price, string imgsrc)
        {
            ID = id;
            Name = name;
            TypeID = typeid;
            UnitID = unitid;
            Price = price;
            if (imgsrc == null)
                ImageSource = Environment.CurrentDirectory + "\\drink_default.png";
            else
                ImageSource = Environment.CurrentDirectory + imgsrc;
        }

        public static string GenerateID()
        {
            IDatabase sqldb = new SQLDatabase();
            string currentID = null;
            try
            {
                sqldb.Open();
                SqlDataReader reader = sqldb.ExcuteReader("SELECT MAX(MaMatHang) FROM MATHANG");
                while(reader.Read())
                {
                    currentID = reader.GetString(0);
                }
                if (currentID != null)
                {
                    string prefix = currentID.Substring(0, 2);
                    int no = Convert.ToInt32(currentID.Substring(2,3));
                    return prefix + (++no).ToString("000");
                }
            }
            catch(SqlException e)
            {
                Debug.LogOutput("SqlException in ProductModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }
            return null;
        }

        public static void UpdateDatabase(ProductModel product)
        {
            IDatabase sqldb = new SQLDatabase();
            try
            {
                sqldb.Open();
                sqldb.ExcuteNonQuery("UPDATE MATHANG SET TenMatHang = N'" + product.Name + "', MaLoaiMatHang='" + product.TypeID + "', MaDonViTinh='" + product.UnitID+"', TriGia=" + product.Price + ", HinhAnh='" + product.ImageSource + "' WHERE MaMatHang='" + product.ID+"'");
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in ProductModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }
        }

        public static void RemoveFromDatabase(string productID)
        {
            IDatabase sqldb = new SQLDatabase();
            try
            {
                sqldb.Open();
                sqldb.ExcuteNonQuery("UPDATE MATHANG SET GhiChu='REMOVED' WHERE MaMatHang='" + productID + "'");
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in ProductModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }
        }

        public void SaveToDatabase()
        {
            IDatabase sqldb = new SQLDatabase();
            try
            {
                sqldb.Open();
                sqldb.ExcuteNonQuery("INSERT INTO MATHANG VALUES('" + ID + "', N'" + Name + "', '" + TypeID + "', '" + UnitID + "', " + Price + ", '" + ImageSource + "')");
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in ProductModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }  
        }
    }
}
