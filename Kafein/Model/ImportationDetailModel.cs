using Kafein.Database;
using Kafein.Utilities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class ImportationDetailModel
    {
        // from database
        public string ID { get; set; }
        public string ImportationID { get; set; }
        public string IngridientID { get; set; }
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

        public ImportationDetailModel()
        {

        }

        public ImportationDetailModel(string id, string importationId, string ingridientId, string unitid, int quantity, double price)
        {
            ID = id;
            ImportationID = importationId;
            IngridientID = ingridientId;
            UnitID = unitid;
            Quantity = quantity;
            Price = price;
        }

        public static string GenerateID(ObservableCollection<dynamic> listDetail)
        {
            string offID = null;
            string currentDateStr = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2);
            int currDate = Convert.ToInt32(currentDateStr);
            if (listDetail.Count > 0)
            {
                string maxid = listDetail[0].ID;
                foreach (ImportationDetailModel bill in listDetail)
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
            SqlDataReader reader = sqldb.ExcuteReader("SELECT Max(MaCTPhieuNhapHang) FROM CHITIETPHIEUNHAPHANG WHERE SUBSTRING(MaCTPhieuNhapHang, 5, 2)='" + month + "'");
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
                    return "CT" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2) + "001";
                }
            }

            return "CT" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString().Substring(2, 2) + "001";
        }

        public static void SaveToDatabase(ImportationDetailModel detail)
        {
            IDatabase sqldb = new SQLDatabase();
            try
            {
                sqldb.Open();
                sqldb.ExcuteNonQuery("INSERT INTO CHITIETPHIEUNHAPHANG VALUES('" + detail.ID + "', '" + detail.ImportationID + "', '" + detail.IngridientID + "', " + detail.Quantity + ", " + detail.Price + ")");
                sqldb.Close();
            }
            catch (SqlException e)
            {
                Debug.LogOutput(e.ToString());
            }
        }
    }
}
