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
    public class IngridientModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string UnitID { get; set; }
        public double Price { get; set; }
        public string ImageSource { get; set; }

        public IngridientModel()
        {

        }

        public IngridientModel(IngridientModel ingridient)
        {
            ID = ingridient.ID;
            Name = ingridient.Name;
            UnitID = ingridient.UnitID;
            Price = ingridient.Price;
            ImageSource = ingridient.ImageSource;
        }

        public IngridientModel(string id, string name, string unitid, double price, string imgsrc)
        {
            ID = id;
            Name = name;
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
                SqlDataReader reader = sqldb.ExcuteReader("SELECT MAX(MaNguyenLieu) FROM NGUYENLIEU");
                while (reader.Read())
                {
                    currentID = reader.GetString(0);
                }
                if (currentID != null)
                {
                    string prefix = currentID.Substring(0, 2);
                    int no = Convert.ToInt32(currentID.Substring(2, 3));
                    return prefix + (++no).ToString("000");
                }
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in IngridientModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }
            return null;
        }

        public static void UpdateDatabase(IngridientModel ingridient)
        {
            IDatabase sqldb = new SQLDatabase();
            try
            {
                sqldb.Open();
                sqldb.ExcuteNonQuery("UPDATE NGUYENLIEU SET TenNguyenLieu = N'" + ingridient.Name + "', MaDonViTinh='" + ingridient.UnitID + "', TriGia=" + ingridient.Price + ", HinhAnh='" + ingridient.ImageSource + "' WHERE MaNguyenLieu='" + ingridient.ID + "'");
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in IngridientModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }
        }

        public static void RemoveFromDatabase(string ingridientID)
        {
            IDatabase sqldb = new SQLDatabase();
            try
            {
                sqldb.Open();
                sqldb.ExcuteNonQuery("UPDATE NGUYENLIEU SET GhiChu='REMOVED' WHERE MaNguyenLieu='" + ingridientID + "'");
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in IngridientModel >> " + e.ToString());
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
                if (ImageSource.Contains(Environment.CurrentDirectory))
                    ImageSource = ImageSource.Remove(0, Environment.CurrentDirectory.Length);
                sqldb.ExcuteNonQuery("INSERT INTO NGUYENLIEU VALUES('" + ID + "', N'" + Name + "', '" + UnitID + "', " + Price + ", '" + ImageSource + "', NULL)");
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in IngridientModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }
        }
    }
}