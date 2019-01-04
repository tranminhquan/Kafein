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
    public class ListExpenditureModel: BaseList<ExpenditureModel>
    {
        private static ListExpenditureModel instance = null;

        public ListExpenditureModel():base()
        {

        }

        public static ListExpenditureModel GetInstance()
        {
            if (instance == null)
                instance = new ListExpenditureModel();
            return instance;
        }

        public void LoadExpenditureReport(int month, int year)
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT MaCTPhieuNhapHang, CHITIETPHIEUNHAPHANG.MaPhieuNhapHang, TenNguyenLieu, NgayLapPhieu, SoLuong, TriGia, ThanhTien" +
                " FROM CHITIETPHIEUNHAPHANG JOIN PHIEUNHAPHANG" +
                " ON CHITIETPHIEUNHAPHANG.MaPhieuNhapHang = PHIEUNHAPHANG.MaPhieuNhapHang" +
                " JOIN NGUYENLIEU ON CHITIETPHIEUNHAPHANG.MaNguyenLieu = NGUYENLIEU.MaNguyenLieu" +
                " WHERE MONTH(NgayLapPhieu) = " + month + " AND YEAR(NgayLapPhieu)=" + year);

            while (reader.Read())
            {
                try
                {
                    string importationdetailid = reader.GetString(0);
                    string importationid = reader.GetString(1);
                    string ingredient = reader.GetString(2);
                    DateTime date = reader.GetDateTime(3);
                    int quantity = reader.GetInt32(4);
                    double value = reader.GetSqlMoney(5).ToDouble();
                    double price = reader.GetSqlMoney(6).ToDouble();

                    this.List.Add(new ExpenditureModel(importationdetailid, importationid, ingredient, date, quantity, value, price));
                }
                catch (SqlException e)
                {
                    Debug.LogOutput(">> Exception at ListRevenueModel: " + e.ToString());
                }
            }
        }

        public void LoadAllExpenditure()
        {
            this.List.Clear();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT MaCTPhieuNhapHang, CHITIETPHIEUNHAPHANG.MaPhieuNhapHang, TenNguyenLieu, NgayLapPhieu, SoLuong, TriGia, ThanhTien" +
                " FROM CHITIETPHIEUNHAPHANG JOIN PHIEUNHAPHANG" +
                " ON CHITIETPHIEUNHAPHANG.MaPhieuNhapHang = PHIEUNHAPHANG.MaPhieuNhapHang" +
                " JOIN NGUYENLIEU ON CHITIETPHIEUNHAPHANG.MaNguyenLieu = NGUYENLIEU.MaNguyenLieu");

            while (reader.Read())
            {
                try
                {
                    string importationdetailid = reader.GetString(0);
                    string importationid = reader.GetString(1);
                    string ingredient = reader.GetString(2);
                    DateTime date = reader.GetDateTime(3);
                    int quantity = reader.GetInt32(4);
                    double value = reader.GetSqlMoney(5).ToDouble();
                    double price = reader.GetSqlMoney(6).ToDouble();

                    this.List.Add(new ExpenditureModel(importationdetailid, importationid, ingredient, date, quantity, value, price));
                }
                catch (SqlException e)
                {
                    Debug.LogOutput(">> Exception at ListRevenueModel: " + e.ToString());
                }
            }
        }
    }
}
