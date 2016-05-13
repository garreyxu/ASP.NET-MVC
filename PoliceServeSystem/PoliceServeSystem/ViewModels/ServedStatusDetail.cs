using System;
using System.ComponentModel.DataAnnotations;

namespace PoliceServeSystem.ViewModels
{
    public class ServedStatusDetail
    {
        #region Properties

        [Required]
        public string WarrantNo { get; set; }
        [Required]
        public string AccusedNo { get; set; }
        [Required]
        public string CaseNo { get; set; }
        [Required]
        public int? ServedTimes { get; set; }
        [Required]
        public DateTime ServedDate { get; set; }
        [Required]
        public string Result { get; set; }
        [Required]
        public string ServedBy { get; set; }
        [Required]
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
