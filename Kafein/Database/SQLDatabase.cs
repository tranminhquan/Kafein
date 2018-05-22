using Kafein.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Database
{
    public class SQLDatabase: IDatabase
    {
        private SqlConnection connection;

        public SQLDatabase()
        {
            connection = this.CreateConnection();
        }

        public SqlConnection CreateConnection()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "kafein.database.windows.net";
                builder.UserID = "kafeinadmin";
                builder.Password = "Kafeinabsoluke123";
                builder.InitialCatalog = "QL_CAPHE";

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                return connection;
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SQL Exception >> " + e);
                return null;
            }
        }

        public void ExcuteNonQuery(string query)
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Debug.Log("SQL Exception when excute non query >> " + e);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public SqlDataReader ExcuteReader(string query)
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader;
                    }
                }
                catch (SqlException e)
                {
                    Debug.Log("SQL Exception when excute reader >> " + e);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void Test()
        {
            String query = "SELECT * FROM TEST WHERE Unit = 'Ly'";
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.LogOutput(reader.GetString(0) + "|\t\t" + reader.GetString(1) + "|\t\t" + reader.GetString(2) + "|\t" + reader.GetSqlMoney(3));
                    }
                }
            }
            connection.Close();
        }

        //public T GetModelFromField(string tableName, string fieldName, string fieldValue)
        //{        
        //    SqlDataReader reader = this.ExcuteReader("SELECT * FROM " + tableName + " WHERE " + fieldName + "='" + fieldValue + "'");
        //    object[] constrParams = new object[reader.FieldCount];
        //    while (reader.Read())
        //    {
        //        for(int i=0; i<reader.FieldCount; i++)
        //        {
        //            constrParams[i] = reader.GetS
        //        }
        //    }
        //    return (T)Activator.CreateInstance(typeof(T), new object[] { weight });
        //}
    }
}
