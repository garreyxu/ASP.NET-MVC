using PoliceServeSystem.Models;
using PoliceServeSystem.ViewModels;

namespace PoliceServeSystem.DAL
{
    public interface IServedInfoDataService
    {
        Served Load(string warrantNo);

        void Save(ServedStatusDetail ssd);
    }
}
