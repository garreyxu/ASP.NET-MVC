using PoliceServeSystem.App_Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace PoliceServeSystem.Helper
{
    public class ConfigurationSettingsReader
    {
        public ConfigurationSettingsReader()
        {
        }
        public static string ConnectionString
        {
            get { return CommonRoutines.DecryptString(ConfigurationManager.AppSettings["ConnectionString:SQL"]); }
        }
        public static string videoConnectionString
        {
            get { return CommonRoutines.DecryptString(ConfigurationManager.AppSettings["ConnectionString:VideoSQL"]); }
        }
        public static string ApplicationHeaderTextEWI
        {
            get { return ConfigurationManager.AppSettings["ApplicationHeaderText:EWI"]; }
        }
        public static string ApplicationHeaderTextAbbreviationEWI
        {
            get { return ConfigurationManager.AppSettings["ApplicationHeaderTextAbbreviation:EWI"]; }
        }
        public static string ApplicationHeaderTextJail
        {
            get { return ConfigurationManager.AppSettings["ApplicationHeaderText:Jail"]; }
        }

        public static string ApplicationHeaderTextQuickEntry
        {
            get { return ConfigurationManager.AppSettings["ApplicationHeaderText:QuickEntry"]; }
        }

        public static string ApplicationHeaderTextAbbreviationJail
        {
            get { return ConfigurationManager.AppSettings["ApplicationHeaderTextAbbreviation:Jail"]; }
        }
        public static string ApplicationHeaderTextAbbreviationQuickEntry
        {
            get { return ConfigurationManager.AppSettings["ApplicationHeaderTextAbbreviation:QuickEntry"]; }
        }

        public static string ApplicationPhysicalRoot
        {
            get
            {
                return ConfigurationManager.AppSettings["ApplicationPhysicalRoot"];
            }
        }
        public static string ApplicationVirtualRoot
        {
            get
            {
                return ConfigurationManager.AppSettings["ApplicationVirtualRoot"];
            }
        }

        public static string ServerLocationSignatureFolder
        {
            get { return ConfigurationManager.AppSettings["ServerLocation:SignatureFolder"]; }
        }
        public static string ServerLocationDocumentsFolder
        {
            get { return ConfigurationManager.AppSettings["ServerLocation:DocumentsFolder"]; }
        }
        public static string ServerLocationStatisticsLog
        {
            get { return ConfigurationManager.AppSettings["ServerLocation:StatisticsLog"]; }
        }
        public static string ServerLocationLocalFolder
        {
            get { return ConfigurationManager.AppSettings["ServerLocation:LocalFolder"]; }
        }
        public static string ServerLocationImportedImgFolder
        {
            get { return ConfigurationManager.AppSettings["ServerLocation:ImportedImgFolder"]; }
        }

        public static string SignatureKey
        {
            get { return "PTGSignature"; }
        }
        //public static string CurrentCounty
        //{
        //    get { return ConfigurationManager.AppSettings["CurrentCounty"]; }
        //}
        //public static string CurrentState
        //{
        //    get { return ConfigurationManager.AppSettings["CurrentState"]; }
        //}

        //public static string CurrentCounty
        //{
        //    get { return GetCountyState("County"); }
        //}
        //public static string CurrentState
        //{
        //    get { return GetCountyState("State"); }
        //}

        //public static string SupervisorApproval
        //{
        //    get { return ConfigurationManager.AppSettings["SupervisorApproval"]; }
        //}

        //public static string CurrentPrinter
        //{
        //    get { return ConfigurationManager.AppSettings["PrinterName"]; }
        //}
        //public static string SMTPHost
        //{
        //    get { return ConfigurationManager.AppSettings["SMTP:Host"]; }
        //}
        //public static int SMTPPort
        //{
        //    get { return Convert.ToInt32(ConfigurationManager.AppSettings["SMTP:Port"]); }
        //}
        //public static bool SMTPUseDefaultCredentials
        //{
        //    get { return ConfigurationManager.AppSettings["SMTP:UseDefaultCredentials"].ToUpper() == "FALSE" ? false : true; }
        //}
        //public static string SMTPUsername
        //{
        //    get { return CommonRoutines.DecryptString(ConfigurationManager.AppSettings["SMTP:Username"]); }
        //}
        //public static string SMTPPassword
        //{
        //    get { return CommonRoutines.DecryptString(ConfigurationManager.AppSettings["SMTP:Password"]); }
        //}
        //public static string SendRecallEmail_To
        //{
        //    get { return CommonRoutines.DecryptString(ConfigurationManager.AppSettings["SendRecallEmail_To"]); }
        //}
        //public static string GetCountyState(string type)
        //{
        //    string strrtn = "";
        //    DataTable dt = new DataTable();
        //    dt = new AddressConfigurationBase_DAL().GetCountyState();
        //    try
        //    {
        //        if (type == "County")
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                strrtn = dt.Rows[0]["County"].ToString();
        //            }
        //            else
        //            {
        //                strrtn = ConfigurationManager.AppSettings["CurrentCounty"];
        //            }
        //        }
        //        else
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                strrtn = dt.Rows[0]["State"].ToString();
        //            }
        //            else
        //            {
        //                strrtn = ConfigurationManager.AppSettings["CurrentState"];
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return strrtn;
        //}




    }
}