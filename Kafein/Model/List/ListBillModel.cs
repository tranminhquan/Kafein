using Kafein.Database;
using Kafein.Model.SalesNPay;
using Kafein.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListBillModel: BaseList<BillModel>
    {
        private static ListBillModel instance = null;
        public ListBillModel(): base()
        {

        }

        public static ListBillModel GetInstance()
        {
            if (instance == null)
                instance = new ListBillModel();
            return instance;
        }

        public ObservableCollection<object> ListID
        {
            get { return this.GetCollectionOfField("ID"); }
        }

        public ObservableCollection<object> ListDeskNo
        {
            get { return this.GetCollectionOfField("DeskNo"); }
        }

        public ObservableCollection<object> ListDate
        {
            get { return this.GetCollectionOfField("Date"); }
        }

        public ObservableCollection<object> ListPrice
        {
            get { return this.GetCollectionOfField("Price"); }
        }

        public void LoadAllBill()
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM HOADON");

            while(reader.Read())
            {
                try
                {
                    string id = reader.GetString(0);
                    int deskno = reader.GetInt32(1);
                    DateTime date = reader.GetDateTime(2);
                    double price = reader.GetSqlMoney(3).ToDouble();

                    BillModel bill = new BillModel(id, deskno, date, price);
                    this.List.Add(bill);
                }
                catch (SqlException e)
                {
                    Debug.LogOutput(">> SqlException at ListBillModel: " + e.ToString());
                }
            }
        }

        public void LoadBillFromMonth(int month)
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM HOADON WHERE Month(NgayLapHoaDon) = " + month);
            while (reader.Read())
            {
                try
                {
                    string id = reader.GetString(0);
                    int deskno = reader.GetInt16(1);
                    DateTime date = reader.GetDateTime(2);
                    double price = reader.GetSqlMoney(4).ToDouble();

                    BillModel bill = new BillModel(id, deskno, date, price);
                    this.List.Add(bill);
                }
                catch (SqlException)
                {

                }
            }
        }

        public void LoadBillFromDay(int day)
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM HOADON WHERE Day(NgayLapHoaDon) = " + day);
            while (reader.Read())
            {
                try
                {
                    string id = reader.GetString(0);
                    int deskno = reader.GetInt16(1);
                    DateTime date = reader.GetDateTime(2);
                    double price = reader.GetSqlMoney(4).ToDouble();

                    BillModel bill = new BillModel(id, deskno, date, price);
                    this.List.Add(bill);
                }
                catch (SqlException)
                {

                }
            }
        }

        public void LoadBillFromDayAndMonth(int day, int month)
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM HOADON WHERE Day(NgayLapHoaDon) = " + day + " AND Month(NgayLapHoaDon) = " + month);
            while (reader.Read())
            {
                try
                {
                    string id = reader.GetString(0);
                    int deskno = reader.GetInt16(1);
                    DateTime date = reader.GetDateTime(2);
                    double price = reader.GetSqlMoney(4).ToDouble();

                    BillModel bill = new BillModel(id, deskno, date, price);
                    this.List.Add(bill);
                }
                catch (SqlException e)
                {
                    Debug.LogOutput(">> SqlException at ListBillModel: " + e.ToString());
                }
            }
        }

        // Get total revenue group by day and month
        // First column: "<day> - <month>"
        // Second column: revenue
        public static ObservableCollection<String[]> GetRevenueByDayAndMonth()
        {
            ObservableCollection<String[]> result = new ObservableCollection<string[]>();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT CAST(MONTH(NgayLapHoaDon) AS VARCHAR(2)) + '-' + CAST(YEAR(NgayLapHoaDon) AS VARCHAR(4)), SUM(TongTriGia), MONTH(NgayLapHoaDon), YEAR(NgayLapHoaDon) FROM HOADON GROUP BY CAST(MONTH(NgayLapHoaDon) AS VARCHAR(2)) + '-' + CAST(YEAR(NgayLapHoaDon) AS VARCHAR(4)), MONTH(NgayLapHoaDon), YEAR(NgayLapHoaDon) ORDER BY YEAR(NgayLapHoaDon), MONTH(NgayLapHoaDon)");
            while (reader.Read())
            {
                try
                {
                    string date = reader.GetString(0);         
                    double price = reader.GetSqlMoney(1).ToDouble();
                    int month = reader.GetInt32(2);
                    int year = reader.GetInt32(3);

                    result.Add(new String[] {date, price.ToString(), month.ToString(), year.ToString()});
                }
                catch (SqlException e)
                {
                    Debug.LogOutput(">> SqlException at ListBillModel: " + e.ToString());
                }
            }

            return result;
        }
    }
}
