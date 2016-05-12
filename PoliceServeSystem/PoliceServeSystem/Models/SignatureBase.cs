using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoliceServeSystem.Models
{
    public class SignatureBase
    {
        #region Consts
        public const string TableName = "Signature";
        public const string TableName1 = "tbl_signature";

        public const string SqlPrefix = TableName + ":";
        #endregion

        #region Constructors
        public SignatureBase()
        {
            _Caseno = "";
            _Signmode = "";
            _SignTime = "";
            _UserID = "";
            _Notary_Name = "";
            _ExpirationDate = "";
        }

        #endregion

        #region DBMappedFields
        private System.String _Caseno;
        private System.String _Signmode;
        private System.Byte[] _Signature;
        private System.String _SignTime;
        private System.String _SignatureString;
        private System.String _UserID;
        private System.String _Notary_Name;
        private System.String _ExpirationDate;
        #endregion

        #region Properties
        public System.String Caseno
        {
            get { return _Caseno; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new Exception("String length overflow on " + TableName + ".Caseno.");
                _Caseno = value;
            }
        }

        public System.String Signmode
        {
            get { return _Signmode; }
            set
            {
                if (value != null && value.Length > 2)
                    throw new Exception("String length overflow on " + TableName + ".Signmode.");
                _Signmode = value;
            }
        }

        public System.Byte[] SignatureBytes
        {
            get { return _Signature; }
            set { _Signature = value; }
        }

        public System.String SignTime
        {
            get { return _SignTime; }
            set
            {
                if (value != null && value.Length > 30)
                    throw new Exception("String length overflow on " + TableName + ".SignTime.");
                _SignTime = value;
            }
        }

        public System.String SignatureString
        {
            get { return _SignatureString; }
            set { _SignatureString = value; }
        }
        public System.String UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public System.String Notary_Name
        {
            get { return _Notary_Name; }
            set { _Notary_Name = value; }
        }
        public System.String ExpirationDate
        {
            get { return _ExpirationDate; }
            set { _ExpirationDate = value; }
        }

        #endregion
    }
}