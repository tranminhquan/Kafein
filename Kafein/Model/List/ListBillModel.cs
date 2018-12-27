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

        public static ListBillModel GetInstace()
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
                    if (!reader.IsDBNull(6))
                        continue;

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

        public void LoadBillFromMonth(int month)
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM HOADON WHERE Month(NgayLapHoaDon) = '" + month + "'");
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
            SqlDataReader reader = sqldb.ExcuteReader("SELECT * FROM HOADON WHERE Day(NgayLapHoaDon) = '" + day + "'");
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
    }
}
