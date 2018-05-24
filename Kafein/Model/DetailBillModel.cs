using Kafein.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class DetailBillModel
    {
        // from database
        public string ID { get; set; }
        public string BillID { get; set; }
        public string ProductID { get; set; }
        public string UnitID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        // read-only property
        public string ProductName
        {
            get
            {
                // crawl from database
                IDatabase sqldb = new SQLDatabase();
                return string.Empty; //temp
            }
        }

        public DetailBillModel()
        {

        }

        public DetailBillModel(string id, string billid, string productid, string unitid, int quantity, double price)
        {
            ID = id;
            BillID = billid;
            ProductID = productid;
            UnitID = unitid;
            Quantity = quantity;
            Price = price;
        }

        public static string GenerateID()
        {
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT Max(MaCTHoaDon) FROM CHITIETHOADON");
            while (reader.Read())
            {
                try
                {
                    string currentID = reader.GetString(0);
                    string prefix = currentID.Substring(0, 2);
                    int date = Convert.ToInt16(currentID.Substring(2, 6));
                    int no = Convert.ToInt16(currentID.Substring(8, 3));

                    string currentDateStr = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2);
                    int currDate = Convert.ToInt16(currentDateStr);

                    if (currDate == date)
                    {
                        no++;
                        sqldb.Close();
                        return prefix + date + no;
                    }
                    sqldb.Close();
                    return prefix + currentDateStr + "001";
                }
                catch (SqlNullValueException e)
                {
                    sqldb.Close();
                    return "CT" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2) + "001";
                }
            }

            return "CT" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2) + "001";
        }
    }
}
