using System;

namespace PoliceServeSystem.ViewModels
{
    public class ServedStatusDetail
    {
        #region Properties
        public string WarrantNo { get; set; }
        public string AccusedNo { get; set; }
        public string CaseNo { get; set; }
        public int? ServedTimes { get; set; }
        public DateTime ServedDate { get; set; }
        public string Result { get; set; }
        public string ServedBy { get; set; }
        public string IsServed { get; set; }
        public string AccusedFirstName { get; set; }
        public string AccusedLastName { get; set; }
        public string AccusedMiddleName { get; set; }
        public string AccusedStreet { get; set; }
        public string AccusedCity { get; set; }
        public string AccusedState { get; set; }
        public string AccusedZip { get; set; }
        public string AccusedTel { get; set; }
        public string OffenseName { get; set; }
        public string OffenseType { get; set; }
        public bool IfHasCaseRecord { get; set; }
        #endregion

        #region Constructor
        public ServedStatusDetail()
        {
            WarrantNo = string.Empty;
            CaseNo = string.Empty;
            AccusedNo = string.Empty;
            ServedTimes = 0;
            ServedDate = DateTime.Now;
            Result = string.Empty;
            ServedBy = string.Empty;
            IsServed = string.Empty;
            AccusedFirstName = string.Empty;
            AccusedLastName = string.Empty;
            AccusedMiddleName = string.Empty;
            AccusedStreet = string.Empty;
            AccusedCity = string.Empty;
            AccusedState = string.Empty;
            AccusedZip = string.Empty;
            AccusedTel = string.Empty;
            OffenseName = string.Empty;
            OffenseType = string.Empty;
            IfHasCaseRecord = true;
        }
        #endregion


    }
}   
