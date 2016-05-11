using System;
using PoliceServeSystem.DAL;

namespace PoliceServeSystem.App_Data
{
    public class CommonRoutines
    {

        public static string EncriptString(string originalString)
        {
            string encryptedString = "";
            originalString = originalString.Trim();
            if (originalString.Length == 0)
            {
                encryptedString = "993062892";
            }
            else
            {
                foreach (char currCharacter in originalString)
                {
                    encryptedString += ((Int32)currCharacter).ToString().PadLeft(3, '0');
                }
            }
            return encryptedString;
        }

        public static string DecryptString(string encryptedString)
        {
            string originalString = "";
            if (encryptedString == "993062892")
            {
                originalString = "";
            }
            else
            {
                for (int iCnt = 0; iCnt < encryptedString.Length; iCnt += 3)
                {
                    originalString += ((char)Convert.ToInt32(encryptedString.Substring(iCnt, 3))).ToString();
                }
            }
            return originalString;
        }


        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            byte[] bytesArray;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bytesArray = ms.ToArray();
            }
            return bytesArray;
        }
        public static System.Drawing.Image ByteArrayToImage(byte[] byteArrayIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            ms.Close();
            ms.Dispose();
            return returnImage;
        }

        public static DateTime DbServerDateTime
        {
            get
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                //using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(GetConnection.ConnString))
                using (System.Data.SqlClient.SqlConnection cn = GetConnection.GetSqlConnection())
                {
                    System.Data.SqlClient.SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "SELECT DATEPART(year, GETDATE()) DBYear,DATEPART(month, GETDATE()) DBMonth,	DATEPART(DAY, GETDATE()) DBDate,DATEPART(hour, GETDATE()) DBHour,DATEPART(minute, GETDATE()) DBMinute,DATEPART(second, GETDATE()) DBSecond";
                    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                return new DateTime(Convert.ToInt32(ds.Tables[0].Rows[0]["DBYear"]), Convert.ToInt32(ds.Tables[0].Rows[0]["DBMonth"]), Convert.ToInt32(ds.Tables[0].Rows[0]["DBDate"]), Convert.ToInt32(ds.Tables[0].Rows[0]["DBHour"]), Convert.ToInt32(ds.Tables[0].Rows[0]["DBMinute"]), Convert.ToInt32(ds.Tables[0].Rows[0]["DBSecond"]));
            }
        }
    }
}
