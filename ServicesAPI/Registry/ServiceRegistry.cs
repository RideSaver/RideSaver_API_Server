using DataAccess.DataModels;
using DataAccess.Models;
using ServicesAPI.Data;
using static DataAccess.DataModels.ServiceFeaturesModel;

namespace ServicesAPI.Registry
{
    public class ServiceRegistry : IServiceRegistry
    {
        private readonly ServiceContext _serviceContext;
        public ServiceRegistry(ServiceContext serviceContext)
        {
            _serviceContext = serviceContext;
            RegisterServices();
        }
        public void RegisterServices()
        {
            var uberBLACK = new ServicesModel()
            {
                Id = new Guid("d4abaae7-f4d6-4152-91cc-77523e8165a4"),
                Name = "UberBLACK",
                ClientId = "Uber",
                ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },},
            };

            var uberPOOL = new ServicesModel()
            {
                Id = new Guid("26546650-e557-4a7b-86e7-6a3942445247"),
                Name = "uberPOOL",
                ClientId = "Uber",
                ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },
                   new ServiceFeaturesModel() { Feature = Features.shared },},
            };

            var uberX = new ServicesModel()
            {
                Id = new Guid("2d1d002b-d4d0-4411-98e1-673b244878b2"),
                Name = "uberX",
                ClientId = "Uber",
                ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },},
            };

            _serviceContext.Services.Add(uberX);
            _serviceContext.Services.Add(uberPOOL);
            _serviceContext.Services.Add(uberBLACK);
        }
    }
}
