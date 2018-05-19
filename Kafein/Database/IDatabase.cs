using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Database
{
    public interface IDatabase
    {
        SqlConnection CreateConnection();

        /// <summary>
        /// Thực thi query không trả về kết quả
        /// </summary>
        /// <param name="query"></param>
        void ExcuteNonQuery(string query);

        /// <summary>
        /// Thực thi query có trả về kết quả
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        SqlDataReader ExcuteReader(string query);
    }
}
