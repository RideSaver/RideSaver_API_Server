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
                ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },},
            };

            var uberPOOL = new ServicesModel() // UberPOOL
            {
                Id = new Guid("26546650-e557-4a7b-86e7-6a3942445247"),
                Name = "uberPOOL",
                ClientId = "Uber",
                ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },
                   new ServiceFeaturesModel() { Feature = Features.shared },},
            };

            var uberX = new ServicesModel() // Uberx
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

            //------------------------------------[LYFT SERVICES]-----------------------------------//

            var lyft = new ServicesModel() // Lyft
            {
                Id = new Guid("8fc8558b-acf1-431a-b354-9fb4d7b8ca77"),
                Name = "Lyft",
                ClientId = "Lyft",
                ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },},
            };

            var lyftShared = new ServicesModel() // LyftSHARED
            {
                Id = new Guid("1c4121ce-6018-467c-9e5d-edf31691b125"),
                Name = "Lyft Shared",
                ClientId = "Lyft",
                ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },
                   new ServiceFeaturesModel() { Feature = Features.shared },},
            };

            var lyftXL = new ServicesModel() // LyftXL
            {
                Id = new Guid("779da75e-2c13-49bc-8dda-e2105904c837"),
                Name = "LyftXL",
                ClientId = "Lyft",
                ProviderId = new Guid(),
                ServiceFeatures = new List<ServiceFeaturesModel>
                {  new ServiceFeaturesModel() { Feature = Features.professional_driver },},
            };

            var LyftLUX = new ServicesModel() // LyftLUX
            {
                Id = new Guid("fc1e5808-d7d0-41c3-a320-cea94cc8a27a"),
                Name = "LyftLUX",
                ClientId = "Lyft",
                ProviderId = new Guid(),
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
