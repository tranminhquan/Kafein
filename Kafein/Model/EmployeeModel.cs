using Kafein.Database;
using Kafein.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.Model
{
    public class EmployeeModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string CardID { get; set; }
        public string Phone { get; set; }
        public string PositionID { get; set; }
        public DateTime StartDate { get; set; }
        public string Shift { get; set; }

        public EmployeeModel()
        {

        }

        public EmployeeModel(EmployeeModel employee)
        {
            ID = employee.ID;
            Name = employee.Name;
            Birthday = employee.Birthday;
            CardID = employee.CardID;
            Phone = employee.Phone;
            PositionID = employee.PositionID;
            StartDate = employee.StartDate;
            Shift = employee.Shift;
        }

        public EmployeeModel(string id, string name, DateTime birthday, string cardid, string phone, string positionId, DateTime startDate, string shift)
        {
            ID = id;
            Name = name;
            Birthday = birthday;
            CardID = cardid;
            Phone = phone;
            PositionID = positionId;
            StartDate = startDate;
            Shift = shift;
        }

        public static string GenerateID()
        {
            IDatabase sqldb = new SQLDatabase();
            string currentID = null;
            try
            {
                sqldb.Open();
                SqlDataReader reader = sqldb.ExcuteReader("SELECT MAX(MaNhanVien) FROM NHANVIEN");
                while (reader.Read())
                {
                    currentID = reader.GetString(0);
                }
                if (currentID != null)
                {
                    string prefix = currentID.Substring(0, 2);
                    int no = Convert.ToInt32(currentID.Substring(2, 3));
                    return prefix + (++no).ToString("000");
                }
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in EmployeeModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }
            return null;
        }

        public static void UpdateDatabase(EmployeeModel employee)
        {
            IDatabase sqldb = new SQLDatabase();
            try
            {
                sqldb.Open();
                sqldb.ExcuteNonQuery("UPDATE NHANVIEN SET HoTen = N'" + employee.Name + "', NgaySinh='" + employee.Birthday + "', CMND=" + employee.CardID +", SoDienThoai='" + employee.Phone + 
                                     "', MaChucVu=" + employee.PositionID + "', NgayVaoLam=" + employee.StartDate + "', NgayVaoLam=" + employee.StartDate + "', Ca=" + employee.Shift +
                                     "' WHERE MaMatHang='" + employee.ID + "'");
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in EmployeeModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }
        }

        public static void RemoveFromDatabase(string employeeID)
        {
            IDatabase sqldb = new SQLDatabase();
            try
            {
                sqldb.Open();
                sqldb.ExcuteNonQuery("UPDATE NHANVIEN SET GhiChu='REMOVED' WHERE MaNhanVien='" + employeeID + "'");
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in EmployeeModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }
        }

        public void SaveToDatabase()
        {
            IDatabase sqldb = new SQLDatabase();
            try
            {
                sqldb.Open();
                sqldb.ExcuteNonQuery("INSERT INTO NHANVIEN VALUES('" + ID + "', N'" + Name + "', '" + Birthday + "', " + CardID + ", '" + Phone + ", '" + PositionID +
                                     ", '" + StartDate + ", '" + Shift + "', NULL)");
            }
            catch (SqlException e)
            {
                Debug.LogOutput("SqlException in EmployeeModel >> " + e.ToString());
            }
            finally
            {
                sqldb.Close();
            }
        }
    }
}
