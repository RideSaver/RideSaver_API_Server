using DataAccess.DataModels;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using RideSaver.Server.Models;
using ServicesAPI.Data;
using static RideSaver.Server.Models.RideService;
using ServiceProvider = RideSaver.Server.Models.ServiceProvider;

namespace ServicesAPI.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ServiceContext _serviceContext;
        public ServiceRepository(ServiceContext serviceContext) => _serviceContext = serviceContext;

        public async Task<List<RideService>> GetAvailableServices()
        {
            var _servicesList = await _serviceContext.Services.ToListAsync(); // Returns a list of serviceModel
            var services = new List<RideService>();
            foreach (var serviceModel in _servicesList)
            {
                var service = new RideService()
                {
                    Id = serviceModel.Id,
                    DisplayName = serviceModel.Name,
                    Provider = serviceModel.ProviderId,
                    Features = ConvertServiceFeaturesModelToFeatures(serviceModel.ServiceFeatures!)
                };

                services.Add(service);
            };
            return services;
        }

        public async Task<List<ServiceProvider>> GetAvailableProviders()
        {
            var serviceProviders = new List<ServiceProvider>();
            var _providersList = await _serviceContext.Providers.ToListAsync();
            foreach (var provider in _providersList)
            {
                var serviceProvider = new ServiceProvider()
                {
                    Id = provider.Id,
                    DisplayName = provider.Name,
                    Security = new AuthorizationMethod()
                    {
                        Authorization = AuthorizationMethod.AuthorizationEnum.NoneEnum,
                        AuthorizationUrl = null
                    },
                };

                serviceProviders.Add(serviceProvider);
            }
            return serviceProviders;
        }

        private static List<FeaturesEnum> ConvertServiceFeaturesModelToFeatures(ICollection<ServiceFeaturesModel> serviceFeatures)
        {
            var serviceFeaturesList = new List<FeaturesEnum>();
            FeaturesEnum feature = new();
            if (serviceFeatures is null) return null;
            foreach (var sF in serviceFeatures)
            {
                switch (sF.Feature)
                {
                    case ServiceFeaturesModel.Features.professional_driver: feature = FeaturesEnum.ProfessionalEnum; break;
                    case ServiceFeaturesModel.Features.self_driving: feature = FeaturesEnum.DriverlessEnum; break;
                    case ServiceFeaturesModel.Features.shared: feature = FeaturesEnum.SharedEnum; break;
                    case ServiceFeaturesModel.Features.cash: feature = FeaturesEnum.AcceptCashEnum; break;
                }
                serviceFeaturesList.Add(feature);
            }
            return serviceFeaturesList;
        }
    }
}
