using System.Data.SqlClient;
using System.Configuration;
using PoliceServeSystem.App_Data;

namespace PoliceServeSystem.DAL
{
    public class GetConnection
    {
        
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(CommonRoutines.DecryptString(ConfigurationManager.AppSettings["ConnectionString:SQL"]));
        }

    }
}
