using PoliceServeSystem.Models;
using PoliceServeSystem.ViewModels;

namespace PoliceServeSystem.Builders
{
    public interface IServedStatusDetailBuilder
    {
        ServedStatusDetail Build(Served served); 
    }
}
