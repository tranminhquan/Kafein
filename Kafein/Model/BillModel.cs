using Kafein.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.SalesNPay
{
    public class BillModel
    {
        // from database
        public string ID { get; set; }
        public int DeskNo { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }

        // for need
        public string OrderTime
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
        }

        public BillModel()
        {

        }

        public BillModel(string id, int deskno, DateTime date, double price)
        {
            ID = id;
            DeskNo = deskno;
            Date = date;
            Price = price;
        }

        public static string GenerateID()
        {
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT Max(SoHoaDon) FROM HOADON");
            
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
                    return "HD" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2) + "001";
                }
            }
            return null;
            //sqldb.Close();
            //return "HD" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2) + "001";
        }
    }
}
