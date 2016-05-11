using PoliceServeSystem.Models;
using System;
using System.Data.SqlClient;

namespace PoliceServeSystem.DAL.DataAdapters
{
    public class AccusedInfoDataAdapter : IDataAdapter<AccusedInfo>
    {
        public void Materialize(AccusedInfo accusedInfo, SqlDataReader sqlDataReader)
        {
            accusedInfo.CaseNo = Convert.ToString(sqlDataReader["CaseNO"]);
            accusedInfo.AccusedNo = Convert.ToString(sqlDataReader["AccusedNO"]);
            accusedInfo.Acc_FirstName = Convert.ToString(sqlDataReader["Acc_FirstName"]);
            accusedInfo.Acc_LastName = Convert.ToString(sqlDataReader["Acc_LastName"]);
            accusedInfo.Acc_add = Convert.ToString(sqlDataReader["Acc_add"]);
            accusedInfo.Acc_city = Convert.ToString(sqlDataReader["Acc_city"]);
            accusedInfo.Acc_st = Convert.ToString(sqlDataReader["Acc_st"]);
            accusedInfo.Acc_zip = Convert.ToString(sqlDataReader["Acc_zip"]);
            accusedInfo.Acc_yadd = Convert.ToString(sqlDataReader["Acc_yadd"]);
            accusedInfo.Acc_ssn = Convert.ToString(sqlDataReader["Acc_ssn"]);
            accusedInfo.Acc_hphone = Convert.ToString(sqlDataReader["Acc_hphone"]);
            accusedInfo.Acc_wphone = Convert.ToString(sqlDataReader["Acc_wphone"]);
            accusedInfo.Acc_fax = Convert.ToString(sqlDataReader["Acc_fax"]);
            accusedInfo.Acc_email = Convert.ToString(sqlDataReader["Acc_email"]);
            accusedInfo.Acc_age = Convert.ToString(sqlDataReader["Acc_age"]);
            accusedInfo.Acc_sex = Convert.ToString(sqlDataReader["Acc_sex"]);
            accusedInfo.Acc_weight = Convert.ToString(sqlDataReader["Acc_weight"]);
            accusedInfo.Acc_height = Convert.ToString(sqlDataReader["Acc_height"]);
            accusedInfo.Acc_hair = Convert.ToString(sqlDataReader["Acc_hair"]);
            accusedInfo.Acc_eye = Convert.ToString(sqlDataReader["Acc_eye"]);
            accusedInfo.Acc_race = Convert.ToString(sqlDataReader["Acc_race"]);
            accusedInfo.Acc_dob = Convert.ToString(sqlDataReader["Acc_dob"]);
            accusedInfo.Acc_alias = Convert.ToString(sqlDataReader["Acc_alias"]);
            accusedInfo.Acc_auto = Convert.ToString(sqlDataReader["Acc_auto"]);
            accusedInfo.Acc_tag = Convert.ToString(sqlDataReader["Acc_tag"]);
            accusedInfo.Acc_mental = Convert.ToString(sqlDataReader["Acc_mental"]);
            accusedInfo.Acc_hist = Convert.ToString(sqlDataReader["Acc_hist"]);
            accusedInfo.Acc_risk = Convert.ToString(sqlDataReader["Acc_risk"]);
            accusedInfo.Acc_threat = Convert.ToString(sqlDataReader["Acc_threat"]);
            accusedInfo.Acc_flight = Convert.ToString(sqlDataReader["Acc_flight"]);
            accusedInfo.Acc_prob = Convert.ToString(sqlDataReader["Acc_prob"]);
            accusedInfo.Acc_par = Convert.ToString(sqlDataReader["Acc_par"]);
            accusedInfo.Acc_agg = Convert.ToString(sqlDataReader["Acc_agg"]);
            accusedInfo.Acc_bond = Convert.ToString(sqlDataReader["Acc_bond"]);
            accusedInfo.Acc_Felony = Convert.ToString(sqlDataReader["Acc_Felony"]);
            accusedInfo.Acc_FamilyViolence = Convert.ToString(sqlDataReader["Acc_FamilyViolence"]);
            accusedInfo.Acc_Misdemeanor = Convert.ToString(sqlDataReader["Acc_Misdemeanor"]);
            accusedInfo.Acc_Condition = Convert.ToString(sqlDataReader["Acc_Condition"]);
            accusedInfo.Acc_VOORT = Convert.ToString(sqlDataReader["Acc_VOORT"]);
            accusedInfo.Acc_BondDollar = Convert.ToString(sqlDataReader["Acc_BondDollar"]);
            accusedInfo.Acc_BondText = Convert.ToString(sqlDataReader["Acc_BondText"]);
            accusedInfo.Acc_MiddleName = Convert.ToString(sqlDataReader["Acc_MiddleName"]);
            accusedInfo.acc_LiveOutGA = Convert.ToString(sqlDataReader["acc_LiveOutGA"]);
            accusedInfo.acc_TieOutGA = Convert.ToString(sqlDataReader["acc_TieOutGA"]);
            accusedInfo.acc_PhotoID = Convert.ToString(sqlDataReader["acc_PhotoID"]);
            accusedInfo.acc_GangInfo = Convert.ToString(sqlDataReader["acc_GangInfo"]);
            accusedInfo.acc_Sid = Convert.ToString(sqlDataReader["acc_Sid"]);
            accusedInfo.Acc_Repeatoffender = Convert.ToString(sqlDataReader["Acc_Repeatoffender"]);
            accusedInfo.Acc_RepeatSexualOffender = Convert.ToString(sqlDataReader["Acc_RepeatOffender"]);
            accusedInfo.Acc_ResidenceStatus = Convert.ToString(sqlDataReader["Acc_ResidenceStatus"]);
            if (Convert.ToString(sqlDataReader["Acc_Translate"]) == "") { accusedInfo.Acc_Translate = 0; }
            else { accusedInfo.Acc_Translate = Convert.ToInt16(sqlDataReader["Acc_Translate"]); }
            //if (Convert.ToString(sqlDataReader["Acc_Def"]) == "") { accusedInfo.Acc_Def = 0; }
            //else { accusedInfo.Acc_Def = Convert.ToInt16(sqlDataReader["Acc_Def"]); } 
        }
    }
}