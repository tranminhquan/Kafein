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
    public class ListRevenueModel: BaseList<RevenueModel>
    {
        private static ListRevenueModel instance = null;

        public ListRevenueModel():base()
        {

        }

        public static ListRevenueModel GetInstance()
        {
            if (instance == null)
                instance = new ListRevenueModel();
            return instance;
        }

        public void LoadRevenueReport(int month, int year)
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT MaCTHoaDon, CHITIETHOADON.SoHoaDon, TenMatHang, NgayLapHoaDon, SoLuong, TriGia, ThanhTien" +
                " FROM CHITIETHOADON" +
                " JOIN HOADON ON CHITIETHOADON.SoHoaDon = HOADON.SoHoaDon" +
                " JOIN MATHANG ON CHITIETHOADON.MaMatHang = MATHANG.MaMatHang" +
                " WHERE MONTH(NgayLapHoaDon) = " + month + " AND YEAR(NgayLapHoaDon)=" + year);

            while(reader.Read())
            {
                try
                {
                    string detailbillid = reader.GetString(0);
                    string billid = reader.GetString(1);
                    string productname = reader.GetString(2);
                    DateTime date = reader.GetDateTime(3);
                    int quantity = reader.GetInt32(4);
                    double value = reader.GetSqlMoney(5).ToDouble();
                    double price = reader.GetSqlMoney(6).ToDouble();

                    this.List.Add(new RevenueModel(detailbillid, billid, productname, date, quantity, value, price));
                }
                catch(SqlException e)
                {
                    Debug.LogOutput(">> Exception at ListRevenueModel: " + e.ToString());
                }
            }
        }
    }
}
