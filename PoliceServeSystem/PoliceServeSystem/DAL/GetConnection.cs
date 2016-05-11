using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
