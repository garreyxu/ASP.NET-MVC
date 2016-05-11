using System;

namespace PoliceServeSystem.Models
{
    public class OffenseInfo
    {
        public string CaseNo { get; set; }
        public string OffenseNo { get; set; }
        public string KeyWord { get; set; }
        public string OffenseCode { get; set; }
        public string OffenseName { get; set; }
        public string OffenseAdd { get; set; }
        public string OffenseCity { get; set; }
        public string OffenseCounty { get; set; }
        public string OffenseState { get; set; }
        public string OffenseZip { get; set; }
        public DateTime? OffenseStartDate { get; set; }
        public DateTime? OffenseStartTime { get; set; }
        public DateTime? OffenseEndDate { get; set; }
        public DateTime? OffenseEndTime { get; set; }
        public string OffenseUnlawBehavior { get; set; }
        public string OffenseDescription { get; set; }
        public string AccusedNo { get; set; }
        public string AccusedName { get; set; }
        public string FamilyViolence { get; set; }
        public string Felony { get; set; }
        public int OffenseId { get; set; }
        public string OffenseCnt { get; set; }
    }
}
