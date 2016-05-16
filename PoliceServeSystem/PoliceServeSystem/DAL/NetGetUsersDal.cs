using System;
using System.Data;
using System.Data.SqlClient;
using PoliceServeSystem.Models;
using PoliceServeSystem.App_Data;


namespace PoliceServeSystem.DAL
{
    public class NetGetUsersDal
    {
        public Users GetUser(string loginId, string password)
        {
            try
            {
                using (SqlConnection sqlConn = GetConnection.GetSqlConnection())
                {
                    sqlConn.Open();
                    using ( var cmd = new SqlCommand("Net_GetUsers", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@LoginID", SqlDbType.VarChar, 50).Value = loginId;
                        cmd.Parameters.Add("@Password", SqlDbType.VarChar, 250).Value = CommonRoutines.EncriptString(password);

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                var user = new Users();
                                ReadData(user, sqlDataReader);
                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't get users" + ex);
            }
            return null;
        }
        
        private void ReadData(Users user, SqlDataReader sqlDataReader)
        {
            user.Userid = Convert.ToString(sqlDataReader["Userid"]);
            user.LoginId = Convert.ToString(sqlDataReader["LoginID"]);
            user.Password = Convert.ToString(sqlDataReader["Password"]);
            user.UserRole = Convert.ToString(sqlDataReader["UserRole"]);
            user.UserRoleLevel = Convert.ToInt16(sqlDataReader["UserRoleLevel"]);
            user.UserName = Convert.ToString(sqlDataReader["UserName"]);
            user.PExpire = Convert.ToInt16(sqlDataReader["PExpire"]);
            user.Street = Convert.ToString(sqlDataReader["Street"]);
            user.City = Convert.ToString(sqlDataReader["City"]);
            user.State = Convert.ToString(sqlDataReader["State"]);
            user.WorkPhone = Convert.ToString(sqlDataReader["WorkPhone"]);
            user.Division = Convert.ToString(sqlDataReader["Division"]);
            user.AgencyName = Convert.ToString(sqlDataReader["AgencyName"]);
            user.FirstName = Convert.ToString(sqlDataReader["FirstName"]);
            user.MiddleName = Convert.ToString(sqlDataReader["MiddleName"]);
            user.LastName = Convert.ToString(sqlDataReader["LastName"]);
            user.HomePhone = Convert.ToString(sqlDataReader["HomePhone"]);
            user.Fax = Convert.ToString(sqlDataReader["Fax"]);
            user.Email = Convert.ToString(sqlDataReader["Email"]);
            user.Zip = Convert.ToString(sqlDataReader["zip"]);
            user.BadgeNo = Convert.ToString(sqlDataReader["BadgeNo"]);
            user.UserActive = Convert.ToInt16(sqlDataReader["UserActive"]);
            user.IsAgencyExpired = Convert.ToInt16(sqlDataReader["IsAgencyExpired"]);
            user.Agency = Convert.ToString(sqlDataReader["Agency"]);
            user.NotificationType = Convert.ToString(sqlDataReader["NotificationType"]);
            if (string.IsNullOrEmpty(user.NotificationType))
                user.NotificationType = "None";
            
        }
    }
}
