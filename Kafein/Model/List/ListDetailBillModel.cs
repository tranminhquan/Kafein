using Kafein.Database;
using Kafein.Utilities;
using Kafein.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model.List
{
    public class ListDetailBillModel: BaseList<DetailBillItemViewModel>
    {
        //private static ListDetailBillModel instance = null;
        
        public ListDetailBillModel(): base()
        {

        }

        //public static ListDetailBillModel GetInstance()
        //{
        //    if (instance == null)
        //        instance = new ListDetailBillModel();
        //    return instance;
        //}

        public ObservableCollection<dynamic> ListDetail
        {
            get { return this.GetCollectionOfField("DetailBillModel"); }
        }

        public static int GetSumDetailBill()
        {
            IDatabase sqldb = new SQLDatabase();
            int result = 0;
            try
            {
                sqldb.Open();
                SqlDataReader reader = sqldb.ExcuteReader("SELECT COUNT(MaCTHoaDon) FROM CHITIETHOADON");
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
            }
            catch(SqlException e)
            {
                Debug.LogOutput("Sql exception in ListDetailBillModel >> " + e.ToString());
                return 0;
            }
            finally
            {
                sqldb.Close();
            }
            return result;
        }

        public static int GetSumDetailBillFromProduct(string productID)
        {
            IDatabase sqldb = new SQLDatabase();
            int result = 0;
            try
            {
                sqldb.Open();
                SqlDataReader reader = sqldb.ExcuteReader("SELECT COUNT(MaCTHoaDon) FROM CHITIETHOADON WHERE MaMatHang = '" + productID +"'");
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
            }
            catch (SqlException e)
            {
                Debug.LogOutput("Sql exception in ListDetailBillModel >> " + e.ToString());
                return 0;
            }
            finally
            {
                sqldb.Close();
            }
            return result;
        }
    }
}
