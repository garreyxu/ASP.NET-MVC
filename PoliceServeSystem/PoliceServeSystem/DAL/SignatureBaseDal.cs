using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Antlr.Runtime.Tree;
using PoliceServeSystem.Models;

namespace PoliceServeSystem.DAL
{
    public class SignatureBaseDal
    {
        public bool Net_InsertSignature(SignatureBase signatureBase)
        {
            try
            {
                using (SqlConnection sqlCon = GetConnection.GetSqlConnection())
                {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand("InsertSignatureMVC", sqlCon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CaseNo", SqlDbType.NVarChar, 50).Value = signatureBase.Caseno;
                        cmd.Parameters.Add("@SignMode", SqlDbType.NVarChar, 2).Value = signatureBase.Signmode;
                        cmd.Parameters.Add("@Signature", SqlDbType.Image).Value = signatureBase.SignatureBytes;
                        cmd.Parameters.Add("@SignTime", SqlDbType.Char, 30).Value = signatureBase.SignTime;
                        cmd.Parameters.Add("@SignatureString", SqlDbType.VarChar, 8000).Value =
                            signatureBase.SignatureString;
                        cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 20).Value = signatureBase.UserID;
                        cmd.Parameters.Add("@Notary_Name", SqlDbType.VarChar, 50).Value = signatureBase.Notary_Name;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool Exist(string caseNo)
        {
            var cnt = 0;
            
            try
            {
                using (SqlConnection sqlCon = GetConnection.GetSqlConnection())
                {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand("SignExistMVC", sqlCon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@CaseNo", SqlDbType.VarChar, 50).Value = caseNo;
                        cmd.Parameters.Add("@SignMode", SqlDbType.VarChar, 10, "0");

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                cnt = Convert.ToInt32(sqlDataReader["SignCount"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't access get if sign is exist " + ex);
            }
            return cnt != 0;
        }

        public static bool Exist(string caseNo, string signMode)
        {
            var cnt = 0;

            try
            {
                using (SqlConnection sqlCon = GetConnection.GetSqlConnection())
                {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand("SignExistMVC", sqlCon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@CaseNo", SqlDbType.VarChar, 50).Value = caseNo;
                        cmd.Parameters.Add("@SignMode", SqlDbType.VarChar, 10).Value = signMode;

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                cnt = Convert.ToInt32(sqlDataReader["SignCount"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't access get if sign is exist " + ex);
            }
            return cnt != 0;
        }

        public bool SignatureExist(string caseNo, string signMode)
        {
            var cnt = 0;

            try
            {
                using (SqlConnection sqlCon = GetConnection.GetSqlConnection())
                {
                    sqlCon.Open();
                    using (var cmd = new SqlCommand("SignExistMVC", sqlCon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@CaseNo", SqlDbType.VarChar, 50).Value = caseNo;
                        cmd.Parameters.Add("@SignMode", SqlDbType.VarChar, 10).Value = signMode;

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                cnt = Convert.ToInt32(sqlDataReader["SignCount"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't access get if sign is exist " + ex);
            }
            return cnt != 0;
        }

        //public bool UpdateCaseDetails(CaseDetails caseDetailsObj)
        //{
        //    cmd = new SqlCommand("UpdateCaseDetails", Conn.cn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@CaseNo", caseDetailsObj.CaseNO);
        //    cmd.Parameters.AddWithValue("@Disposition", caseDetailsObj.Disposition);
        //    try
        //    {
        //        Conn.cn.Open();
        //        cmd.ExecuteNonQuery();
        //        Conn.cn.Close();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        Conn.cn.Close();
        //        return false;
        //    }
        //    finally
        //    {
        //        Conn.cn.Close();
        //    }
        //}


        //public bool GenerateWarrantNumberForAll(SignatureBase obj)
        //{
        //    cmd = new SqlCommand("Net_GenerateWarrantForAll", Conn.cn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@CaseNo", obj.Caseno);
        //    try
        //    {
        //        Conn.cn.Open();
        //        cmd.ExecuteNonQuery();
        //        Conn.cn.Close();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        Conn.cn.Close();
        //        return false;
        //    }
        //    finally
        //    {
        //        Conn.cn.Close();
        //    }
        //}
    }
}