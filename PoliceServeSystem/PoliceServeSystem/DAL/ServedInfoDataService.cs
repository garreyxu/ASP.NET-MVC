using System;
using System.Data.SqlClient;
using System.Data;
using PoliceServeSystem.Models;
using PoliceServeSystem.DAL.DataAdapters;
using PoliceServeSystem.ViewModels;

namespace PoliceServeSystem.DAL
{
    public class ServedInfoDataService : IServedInfoDataService
    {
        private readonly IDataAdapter<AccusedInfo> _accusedInfoDataAdapter;
        private readonly IDataAdapter<OffenseInfo> _offenseInfoDataAdapter;

        //Dependency Injection;
        public ServedInfoDataService(IDataAdapter<AccusedInfo> accusedInfoDataAdapter, IDataAdapter<OffenseInfo> offenseInfoDataAdapter)
        {
            _accusedInfoDataAdapter = accusedInfoDataAdapter;
            _offenseInfoDataAdapter = offenseInfoDataAdapter;
        }

        public Served Load(string warrantNo)
        {
            try
            {
                using (SqlConnection sqlConn = GetConnection.GetSqlConnection())
                {
                    sqlConn.Open();
                    using (var cmd = new SqlCommand("Net_GetServeInfoMVC", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@warrantNo", warrantNo);
                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while(sqlDataReader.Read())
                            {
                                var served = new Served()
                                {
                                    AccusedInfo = new AccusedInfo(),
                                    OffenseInfo = new OffenseInfo()
                                };
                                ReadData(served, sqlDataReader);
                                return served;
                            }
                        }                            
                    }
                }  
            }
            catch (Exception ex)
            {
                throw new Exception("Could not load data in ServedInfo", ex);
            }

            return null;
        }

        public void Save(ServedStatusDetail ssd)
        {
            try
            {
                using (SqlConnection sqlConn = GetConnection.GetSqlConnection())
                {
                    sqlConn.Open();
                    using (var cmd = new SqlCommand("Net_InsertUpdateServeInfoMVC", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@WarrantNo", SqlDbType.VarChar, 50).Value = ssd.WarrantNo;
                        cmd.Parameters.Add("@IsServed", SqlDbType.VarChar, 10).Value = ssd.SignatureValue != null ? "Yes" : "No";
                        cmd.Parameters.Add("@ServedDate", SqlDbType.DateTime).Value = ssd.ServedDate;
                        cmd.Parameters.Add("@ServedBy", SqlDbType.VarChar, 20).Value = ssd.ServedBy;
                        cmd.Parameters.Add("@Result", SqlDbType.VarChar, 100).Value = ssd.Result;
                        if (ssd.SignatureValue != null)
                        {
                            cmd.Parameters.Add("@SignatureValue", SqlDbType.VarChar, 8000).Value = ssd.SignatureValue;
                        }
                        else
                        {
                            cmd.Parameters.Add("@SignatureValue", SqlDbType.VarChar, 8000).Value = DBNull.Value;
                        }
                        cmd.Parameters.Add("@Comments", SqlDbType.VarChar, 1000).Value = ssd.Comments;

                        cmd.ExecuteNonQuery();
                        //Add save successfully info
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not load data in Accused Object", ex);
            }
        }

        private void ReadData(Served served, SqlDataReader sqlDataReader)
        {
            served.WarrantNo = Convert.ToString(sqlDataReader["WarrantNo"]);
            //warrant hasn't been served.
            if (served.WarrantNo != "")
            {
                served.ServedTimes = Convert.ToInt32(sqlDataReader["ServedTimes"]);
                served.ServedDate = Convert.ToDateTime(sqlDataReader["ServedDate"]);
                served.ServedBy = Convert.ToString(sqlDataReader["ServedBy"]);
                served.IsServed = Convert.ToString(sqlDataReader["IsServed"]);
                served.Result = Convert.ToString(sqlDataReader["Result"]);
                served.Comments = Convert.ToString(sqlDataReader["Comments"]);
            }
            else
            {
                served.ServedTimes = 0;
                served.ServedDate = DateTime.Now;
                served.ServedBy = string.Empty;
                served.IsServed = "0";
                served.Result = string.Empty;
                served.SignatureValue = string.Empty;
                served.Comments = string.Empty;
            }

            _accusedInfoDataAdapter.Materialize(served.AccusedInfo, sqlDataReader);
            _offenseInfoDataAdapter.Materialize(served.OffenseInfo, sqlDataReader);

        }
    }
}