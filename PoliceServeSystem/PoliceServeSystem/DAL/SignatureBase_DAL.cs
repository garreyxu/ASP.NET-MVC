using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PoliceServeSystem.Models;
using System.Data;

namespace PoliceServeSystem.DAL
{
    public class SignatureBase_DAL
    {
        GetConnection Conn = new GetConnection();
        SqlCommand cmd;

        public bool Net_InsertSignature(SignatureBase obj)
        {
            cmd = new SqlCommand("InsertSignature", Conn.cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CaseNo", obj.Caseno);
            cmd.Parameters.AddWithValue("@SignMode", obj.Signmode);
            cmd.Parameters.AddWithValue("@Signature", obj.SignatureBytes);
            cmd.Parameters.AddWithValue("@SignTime", obj.SignTime);
            cmd.Parameters.AddWithValue("@SignatureString", obj.SignatureString);
            cmd.Parameters.AddWithValue("@UserID", obj.UserID);
            cmd.Parameters.AddWithValue("@Notary_Name", obj.Notary_Name);

            try
            {
                Conn.cn.Open();
                int a = cmd.ExecuteNonQuery();
                Conn.cn.Close();
                return true;
            }
            catch (Exception)
            {
                Conn.cn.Close();
                return false;
            }
            finally
            {
                Conn.cn.Close();
            }
        }

        public static bool Exist(string CaseNo)
        {
            int CNT = 0;
            GetConnection Conn = new GetConnection();
            SqlCommand cmd = new SqlCommand("SignExist", Conn.cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CaseNo", CaseNo);
            cmd.Parameters.AddWithValue("@SignMode", "0");

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    CNT = Convert.ToInt16(dt.Rows[0]["SignCount"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not find exist signature for given Case No.", ex);
            }

            if (CNT == 0)
                return false;
            else
                return true;
        }

        public static bool Exist(string CaseNo, string SignMode)
        {
            int CNT = 0;
            GetConnection Conn = new GetConnection();
            SqlCommand cmd = new SqlCommand("SignExist", Conn.cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CaseNo", CaseNo);
            cmd.Parameters.AddWithValue("@SignMode", SignMode);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    CNT = Convert.ToInt16(dt.Rows[0]["SignCount"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not find exist signature for given Case No.", ex);
            }

            if (CNT == 0)
                return false;
            else
                return true;
        }

        public bool SignatureExist(string CaseNo, string SignMode)
        {
            int CNT = 0;
            cmd = new SqlCommand("SignExist", Conn.cn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CaseNo", CaseNo);
            cmd.Parameters.AddWithValue("@SignMode", SignMode);

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    CNT = Convert.ToInt16(dt.Rows[0]["SignCount"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not find exist signature for given Case No.", ex);
            }

            if (CNT == 0)
                return false;
            else
                return true;
        }

        public bool UpdateCaseDetails(CaseDetails caseDetailsObj)
        {
            cmd = new SqlCommand("UpdateCaseDetails", Conn.cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CaseNo", caseDetailsObj.CaseNO);
            cmd.Parameters.AddWithValue("@Disposition", caseDetailsObj.Disposition);
            try
            {
                Conn.cn.Open();
                cmd.ExecuteNonQuery();
                Conn.cn.Close();
                return true;
            }
            catch (Exception)
            {
                Conn.cn.Close();
                return false;
            }
            finally
            {
                Conn.cn.Close();
            }
        }

        public bool GenerateWarrantNumberForAll(SignatureBase obj)
        {
            cmd = new SqlCommand("Net_GenerateWarrantForAll", Conn.cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CaseNo", obj.Caseno);
            try
            {
                Conn.cn.Open();
                cmd.ExecuteNonQuery();
                Conn.cn.Close();
                return true;
            }
            catch (Exception)
            {
                Conn.cn.Close();
                return false;
            }
            finally
            {
                Conn.cn.Close();
            }
        }
    }
}