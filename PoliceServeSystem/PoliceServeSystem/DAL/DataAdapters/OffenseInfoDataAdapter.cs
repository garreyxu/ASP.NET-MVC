using PoliceServeSystem.Models;
using System;
using System.Data.SqlClient;

namespace PoliceServeSystem.DAL.DataAdapters
{
    public class OffenseInfoDataAdapter : IDataAdapter<OffenseInfo>
    {
        public void Materialize(OffenseInfo offenseInfo, SqlDataReader sqlDataReader)
        {
            offenseInfo.CaseNo = Convert.ToString(sqlDataReader["CaseNo"]);
            offenseInfo.OffenseNo = Convert.ToString(sqlDataReader["OffenseNO"]);
            offenseInfo.KeyWord = Convert.ToString(sqlDataReader["KeyWord"]);
            offenseInfo.OffenseCode = Convert.ToString(sqlDataReader["OffenseCode"]);
            offenseInfo.OffenseName = Convert.ToString(sqlDataReader["OffenseName"]);
            offenseInfo.OffenseAdd = Convert.ToString(sqlDataReader["Offense_add"]);
            offenseInfo.OffenseCity = Convert.ToString(sqlDataReader["Offense_city"]);
            offenseInfo.OffenseCounty = Convert.ToString(sqlDataReader["Offense_county"]);
            offenseInfo.OffenseState = Convert.ToString(sqlDataReader["Offense_state"]);
            offenseInfo.OffenseZip = Convert.ToString(sqlDataReader["Offense_zip"]);
            offenseInfo.OffenseStartDate = Convert.ToDateTime(sqlDataReader["Offense_start_date"]);
            offenseInfo.OffenseStartTime = Convert.ToDateTime(sqlDataReader["Offense_start_time"]);
            offenseInfo.OffenseEndDate = Convert.ToDateTime(sqlDataReader["Offense_end_date"]);
            offenseInfo.OffenseEndTime = Convert.ToDateTime(sqlDataReader["Offense_end_time"]);
            offenseInfo.OffenseUnlawBehavior = Convert.ToString(sqlDataReader["Offense_UnlawBehavior"]);
            offenseInfo.OffenseDescription = Convert.ToString(sqlDataReader["Offense_description"]);
            offenseInfo.AccusedNo = Convert.ToString(sqlDataReader["AccusedNo"]);
            offenseInfo.AccusedName = Convert.ToString(sqlDataReader["AccusedName"]);
            offenseInfo.FamilyViolence = Convert.ToString(sqlDataReader["FamilyViolence"]);
            offenseInfo.Felony = Convert.ToString(sqlDataReader["Felony"]);
            offenseInfo.OffenseId = Convert.ToInt32(sqlDataReader["OffenseID"]);
            offenseInfo.OffenseCnt = Convert.ToString(sqlDataReader["OffenseCnt"]);
        }
    }
}