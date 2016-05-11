using System.Web.Mvc;
using Microsoft.Practices.Unity;
using PoliceServeSystem.Builders;
using Unity.Mvc3;
using PoliceServeSystem.DAL;
using PoliceServeSystem.DAL.DataAdapters;
using PoliceServeSystem.Models;

namespace PoliceServeSystem
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IServedInfoDataService, ServedInfoDataService>();
            container.RegisterType<IDataAdapter<AccusedInfo>, AccusedInfoDataAdapter>();
            container.RegisterType<IDataAdapter<OffenseInfo>, OffenseInfoDataAdapter>();
            container.RegisterType<IServedStatusDetailBuilder, ServedStatusDetailBuilder>();

            return container;
        }
    }
}