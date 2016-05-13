using PoliceServeSystem.Models;
using PoliceServeSystem.ViewModels;

namespace PoliceServeSystem.Builders
{
    public class ServedStatusDetailBuilder : IServedStatusDetailBuilder
    {
        public ServedStatusDetail Build(Served served)
        {
            var servedStatusDetail = new ServedStatusDetail
            {
                CaseNo = served.AccusedInfo.CaseNo,
                AccusedNo = served.AccusedInfo.AccusedNo,
                AccusedFirstName = served.AccusedInfo.Acc_FirstName,
                AccusedLastName = served.AccusedInfo.Acc_LastName,
                AccusedMiddleName = served.AccusedInfo.Acc_MiddleName,
                AccusedStreet = served.AccusedInfo.Acc_add,
                AccusedCity = served.AccusedInfo.Acc_city,
                AccusedState = served.AccusedInfo.Acc_st,
                AccusedZip = served.AccusedInfo.Acc_zip,
                AccusedTel = served.AccusedInfo.Acc_hphone,
                
                OffenseName = served.OffenseInfo.OffenseName,
                OffenseType = served.OffenseInfo.Felony,

                WarrantNo = served.WarrantNo,
                ServedTimes = served.ServedTimes,
                ServedDate = served.ServedDate,
                IsServed = served.IsServed,
                Result = served.Result,
                ServedBy = served.ServedBy

            };
            

            return servedStatusDetail;
        }
    }
}