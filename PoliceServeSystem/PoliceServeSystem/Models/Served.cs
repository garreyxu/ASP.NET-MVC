using System;

namespace PoliceServeSystem.Models
{
    public class Served
    {
        public string WarrantNo { get; set; }
        public int? ServedTimes { get; set; }
        public DateTime ServedDate { get; set; }
        public string Result { get; set; }
        public string ServedBy { get; set; }
        public string IsServed { get; set; }

        public AccusedInfo AccusedInfo { get; set; }

        public OffenseInfo OffenseInfo { get; set; }
    }
}
