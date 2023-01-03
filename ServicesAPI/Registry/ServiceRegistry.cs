using DataAccess.DataModels;
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
            //------------------------------------[UBER SERVICES]-----------------------------------//

            var uberBLACK = new ServicesModel() // UberBLACK
            {
                Id = new Guid("d4abaae7-f4d6-4152-91cc-77523e8165a4"),
                Name = "UberBLACK",
                ClientId = "Uber",
                //ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },},
            };

            var uberPOOL = new ServicesModel() // UberPOOL
            {
                Id = new Guid("26546650-e557-4a7b-86e7-6a3942445247"),
                Name = "uberPOOL",
                ClientId = "Uber",
                //ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },
                   new ServiceFeaturesModel() { Feature = Features.shared },},
            };

            var uberX = new ServicesModel() // Uberx
            {
                Id = new Guid("2d1d002b-d4d0-4411-98e1-673b244878b2"),
                Name = "uberX",
                ClientId = "Uber",
                //ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },},
            };

            _serviceContext.Services.Add(uberX);
            _serviceContext.Services.Add(uberPOOL);
            _serviceContext.Services.Add(uberBLACK);

            //------------------------------------[LYFT SERVICES]-----------------------------------//

            var lyft = new ServicesModel() // Lyft
            {
                Id = new Guid("2B2225AD-9D0E-45E0-85FB-378FE2B521E0"),
                Name = "Lyft",
                ClientId = "Lyft",
                //ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },},
            };

            var lyftShared = new ServicesModel() // LyftSHARED
            {
                Id = new Guid("52648E86-B617-44FD-B753-295D5CE9D9DC"),
                Name = "LyftShared",
                ClientId = "Lyft",
                //ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },
                   new ServiceFeaturesModel() { Feature = Features.shared },},
            };

            var lyftXL = new ServicesModel() // LyftXL
            {
                Id = new Guid("BB331ADE-E379-4F12-9AB0-A68AF94D5813"),
                Name = "LyftXL",
                ClientId = "Lyft",
                //ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },},
            };

            var LyftLUX = new ServicesModel() // LyftLUX
            {
                Id = new Guid("B47A0993-DE35-4F86-8DD8-C6462F16F5E8"),
                Name = "LyftLUX",
                ClientId = "Lyft",
                //ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },},
            };

            _serviceContext.Services.Add(lyft);
            _serviceContext.Services.Add(lyftShared);
            _serviceContext.Services.Add(lyftXL);
            _serviceContext.Services.Add(LyftLUX);
        }
    }
}
