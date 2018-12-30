using Kafein.Database;
using Kafein.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kafein.Model.Importation;

namespace Kafein.Model.List
{
    public class ListImportationModel : BaseList<ImportationModel>
    {
        private static ListImportationModel instance = null;
        public ListImportationModel() : base()
        {

        }

        public static ListImportationModel GetInstance()
        {
            if (instance == null)
                instance = new ListImportationModel();
            return instance;
        }

        public static ObservableCollection<String[]> GetExpenditureByDayAndMonth()
        {
            ObservableCollection<String[]> result = new ObservableCollection<String[]>();
            IDatabase sqldb = new SQLDatabase();
            sqldb.Open();
            SqlDataReader reader = sqldb.ExcuteReader("SELECT CAST(MONTH(NgayLapPhieu) AS VARCHAR(2)) + '-' + CAST(YEAR(NgayLapPhieu) AS VARCHAR(4)), sum(TongTriGia), MONTH(NgayLapPhieu), YEAR(NgayLapPhieu) FROM PHIEUNHAPHANG GROUP BY CAST(MONTH(NgayLapPhieu) AS VARCHAR(2)) + '-' + CAST(YEAR(NgayLapPhieu) AS VARCHAR(4)), MONTH(NgayLapPhieu), YEAR(NgayLapPhieu) ORDER BY YEAR(NgayLapPhieu), MONTH(NgayLapPhieu)");
            while (reader.Read())
            {
                try
                {
                    string date = reader.GetString(0);
                    double price = reader.GetSqlMoney(1).ToDouble();
                    int month = reader.GetInt32(2);
                    int year = reader.GetInt32(3);

                    result.Add(new String[] { date, price.ToString(), month.ToString(), year.ToString()});
                }
                catch (SqlException e)
                {
                    Debug.LogOutput(">> SqlException at ListImportationModel: " + e.ToString());
                }
            }

            return result;
        }
    }
}
