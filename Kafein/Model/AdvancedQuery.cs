using Kafein.Database;
using Kafein.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    // Static method for advanced query
    public class AdvancedQuery
    {
        public static ObservableCollection<string[]> GetProductRevenue(int month, int year)
        {
            ObservableCollection<string[]> result = new ObservableCollection<string[]>();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader
                ("SELECT CHITIETHOADON.MaMatHang, MATHANG.TenMatHang, Count(CHITIETHOADON.MaMatHang), Sum(ThanhTien) " +
                "FROM CHITIETHOADON JOIN MATHANG ON MATHANG.MaMatHang = CHITIETHOADON.MaMatHang WHERE SoHoaDon IN" +
                "(SELECT SoHoaDon FROM HOADON WHERE MONTH(NgayLapHoaDon) = " + month + " AND YEAR(NgayLapHoaDon) = " + year +") GROUP BY CHITIETHOADON.MaMatHang, TenMatHang");
            while (reader.Read())
            {
                try
                {
                    string id = reader.GetString(0);
                    string name = reader.GetString(1);
                    string quantity = reader.GetInt32(2).ToString();
                    string price = reader.GetSqlMoney(3).ToString();

                    result.Add(new string[] { id, name, quantity, price });
                   
                }
                catch (SqlException e)
                {
                    Debug.LogOutput(">> Exception in AdvancedQuery: " + e.ToString());
                }
            }

            return result;
        }

        public static ObservableCollection<string[]> GetIngredientExpenditure(int month, int year)
        {
            ObservableCollection<string[]> result = new ObservableCollection<string[]>();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader
                ("SELECT CHITIETPHIEUNHAPHANG.MaNguyenLieu, TenNguyenLieu, Count(CHITIETPHIEUNHAPHANG.MaNguyenLieu), Sum(ThanhTien)" +
                " FROM CHITIETPHIEUNHAPHANG JOIN NGUYENLIEU ON NGUYENLIEU.MaNguyenLieu = CHITIETPHIEUNHAPHANG.MaNguyenLieu" +
                " WHERE MaPhieuNhapHang IN (" +
                "SELECT MaPhieuNhapHang FROM PHIEUNHAPHANG WHERE MONTH(NgayLapPhieu) = " + month + " AND YEAR(NgayLapPhieu) =" + year + ")" +
                "GROUP BY CHITIETPHIEUNHAPHANG.MaNguyenLieu, TenNguyenLieu");
            while (reader.Read())
            {
                try
                {
                    string id = reader.GetString(0);
                    string name = reader.GetString(1);
                    string quantity = reader.GetInt32(2).ToString();
                    string price = reader.GetSqlMoney(3).ToString();

                    result.Add(new string[] { id, name, quantity, price });

                }
                catch (SqlException e)
                {
                    Debug.LogOutput(">> Exception in AdvancedQuery: " + e.ToString());
                }
            }

            return result;
        }
    }
}
