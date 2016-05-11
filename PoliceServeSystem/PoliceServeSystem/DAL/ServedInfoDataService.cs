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

                        cmd.Parameters.AddWithValue("@WarrantNo", ssd.WarrantNo);
                        cmd.Parameters.AddWithValue("@ServedTimes", ssd.ServedTimes+1);
                        cmd.Parameters.AddWithValue("@IsServed", ssd.IsServed);
                        cmd.Parameters.AddWithValue("@ServedDate", ssd.ServedDate);
                        cmd.Parameters.AddWithValue("@ServedBy", ssd.ServedBy);
                        cmd.Parameters.AddWithValue("@Result", ssd.Result);

                        cmd.ExecuteNonQuery();
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
                switch ((string)sqlDataReader["IsServed"])
                {
                    case "Yes":
                        served.IsServed = "1";
                        break;
                    case "No":
                        served.IsServed = "2";
                        break;
                    default:
                        served.IsServed = "0";
                        break;
                }
                served.Result = Convert.ToString(sqlDataReader["Result"]);
            }
            else
            {
                served.ServedTimes = 0;
                served.ServedDate = DateTime.Now;
                served.ServedBy = string.Empty;
                served.IsServed = "0";
                served.Result = string.Empty;
            }

            _accusedInfoDataAdapter.Materialize(served.AccusedInfo, sqlDataReader);
            _offenseInfoDataAdapter.Materialize(served.OffenseInfo, sqlDataReader);

        }
    }
}