using Kafein.Database;
using Kafein.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static string GenerateID(ObservableCollection<BillModel> listBill)
        {
            string offID = null;
            string currentDateStr = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2);
            int currDate = Convert.ToInt32(currentDateStr);
            if (listBill.Count > 0)
            {
                string maxid = listBill[0].ID;
                foreach (BillModel bill in listBill)
                {
                    string id = bill.ID;
                    int compare = String.Compare(maxid, id);
                    int date = Convert.ToInt32(id.Substring(2, 6));
                    if (compare == -1 && (currDate == date))
                    {
                        maxid = id;
                    }
                }
                offID = maxid.Substring(0, 2) + maxid.Substring(2, 6) + (Convert.ToInt32(maxid.Substring(8, 3)) + 1).ToString("000");
            }
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            string month = DateTime.Now.Month.ToString("00");
            SqlDataReader reader = sqldb.ExcuteReader("SELECT Max(SoHoaDon) FROM HOADON WHERE SUBSTRING(SoHoaDon, 5, 2)='" + month + "'");
            
            while (reader.Read())
            {
                try
                {
                    string currentID = reader.GetString(0);
                    string prefix = currentID.Substring(0, 2);
                    int date = Convert.ToInt32(currentID.Substring(2, 6));
                    int no = Convert.ToInt32(currentID.Substring(8, 3));

                    if (currDate == date)
                    {
                        no++;
                        sqldb.Close();
                        string tempid = prefix + date.ToString("000000") + no.ToString("000");
                        if (String.Compare(tempid, offID) == -1)
                            return offID;
                        else
                            return tempid;
                    }
                    sqldb.Close();
                    string tid = prefix + currentDateStr + "001";
                    if (String.Compare(tid, offID) == -1)
                        return offID;
                    else
                        return tid;
                }
                catch (SqlNullValueException e)
                {
                    sqldb.Close();
                    return "HD" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2) + "001";
                }
            }
            return "HD" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2) + "001";
        }

        public static void SaveToDatabase(BillModel bill)
        {
            IDatabase sqldb = new SQLDatabase();
            try
            {
                sqldb.Open();
                sqldb.ExcuteNonQuery("INSERT INTO HOADON VALUES('" + bill.ID + "', " + bill.DeskNo + ",'" + bill.Date + "', " + bill.Price + ")");
                sqldb.Close();
            }
            catch (SqlException e)
            {
                Debug.LogOutput(e.ToString());
            }
        }
    }
}
