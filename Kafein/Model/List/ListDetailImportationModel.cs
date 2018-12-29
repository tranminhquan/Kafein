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
    public class ListDetailImportationModel : BaseList<DetailImportationItemViewModel>
    {
        //private static ListDetailImportationModel instance = null;

        public ListDetailImportationModel() : base()
        {

        }

        //public static ListDetailImportationModel GetInstance()
        //{
        //    if (instance == null)
        //        instance = new ListDetailImportationModel();
        //    return instance;
        //}

        public ObservableCollection<dynamic> ListDetail
        {
            get { return this.GetCollectionOfField("DetailImportationModel"); }
        }

        public static int GetSumDetailImportation()
        {
            IDatabase sqldb = new SQLDatabase();
            int result = 0;
            try
            {
                sqldb.Open();
                SqlDataReader reader = sqldb.ExcuteReader("SELECT COUNT(MaPhieuNhapHang) FROM PHIEUNHAPHANG");
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
            }
            catch (SqlException e)
            {
                Debug.LogOutput("Sql exception in ListDetailImportationModel >> " + e.ToString());
                return 0;
            }
            finally
            {
                sqldb.Close();
            }
            return result;
        }

        public static int GetSumDetailImportationFromIngridient(string ingridientID)
        {
            IDatabase sqldb = new SQLDatabase();
            int result = 0;
            try
            {
                sqldb.Open();
                SqlDataReader reader = sqldb.ExcuteReader("SELECT COUNT(MaCTPhieuNhapHang) FROM CHITIETPHIEUNHAPHANG WHERE MaNguyenLieu = '" + ingridientID + "'");
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
            }
            catch (SqlException e)
            {
                Debug.LogOutput("Sql exception in ListDetailImportationModel >> " + e.ToString());
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
