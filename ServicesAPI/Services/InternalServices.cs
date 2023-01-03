using DataAccess.DataModels;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalAPI;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.Data;
using System.Transactions;
using static DataAccess.DataModels.ServiceFeaturesModel;

namespace ServicesAPI.Services
{
    // Summary: Handles all requests for estimates
    public class InternalServices : InternalAPI.Services.ServicesBase
    {
        private readonly ServiceContext _serviceContext;
        private readonly ILogger<InternalServices> _logger;
        public InternalServices(ServiceContext serviceContext, ILogger<InternalServices> logger)
        {
            _serviceContext = serviceContext;
            _logger = logger;
        }
        public override async Task<ServiceModel> GetServiceByHash(GetServiceByHashRequest request, ServerCallContext context)
        {
            _logger.LogInformation("[ServicesAPI::InternalServices::GetServiceByHash] Method invoked...");

            var EstimateId = new SqlParameter("@EstimateId", request.Hash);
            var service = await _serviceContext.Services.FromSqlRaw($"SELECT * FROM services {EstimateId} = SUBSTRING(HASHBYTES('SHA1', Id), 0, 4))").FirstOrDefaultAsync();
            return new ServiceModel
            {
                Name = service?.Name,
                ClientId = service?.ClientId,
            };
        }
        public override async Task GetServices(Empty request, IServerStreamWriter<ServiceModel> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("[ServicesAPI::InternalServices::GetServices] Method invoked...");

            IList<ServicesModel> services = (IList<ServicesModel>)_serviceContext.Services.ToListAsync();
            foreach (var service in services)
            {
                await responseStream.WriteAsync(new ServiceModel
                {
                    Name = service.Name,
                    ClientId = service.ClientId,
                });
            }
        }
        public override async Task<Empty> RegisterService(RegisterServiceRequest request, ServerCallContext context)
        {
            _logger.LogInformation("[ServicesAPI::InternalServices::RegisterService] Method invoked...");

            var service = new ServicesModel()
            {
                Id = new Guid(request.Id.ToByteArray()),
                Name = request.Name,
                ClientId = request.ClientName,
                ServiceFeatures = ConvertServiceFeaturesToServiceFeaturesModel(request.Features)
            };

            await _serviceContext.Services.AddAsync(service);
            await _serviceContext.SaveChangesAsync();

            _logger.LogInformation("[ServicesAPI::InternalServices::RegisterService] Service sucessfully registered...");

            return Task.FromResult(new Empty());
        }

        public static List<ServiceFeaturesModel> ConvertServiceFeaturesToServiceFeaturesModel(RepeatedField<ServiceFeatures> serviceFeatures)
        {
            var serviceFeaturesList = new List<ServiceFeaturesModel>();
            foreach (var serviceFeature in serviceFeatures)
            {
                var serviceFeaturesModel = new ServiceFeaturesModel();
                switch (serviceFeature)
                {
                    case ServiceFeatures.Shared:
                        serviceFeaturesModel.Feature = Features.shared;
                        break;
                    case ServiceFeatures.Cash:
                        serviceFeaturesModel.Feature = Features.cash;
                        break;
                    case ServiceFeatures.ProfessionalDriver:
                        serviceFeaturesModel.Feature = Features.professional_driver;
                        break;
                    case ServiceFeatures.SelfDriving:
                        serviceFeaturesModel.Feature = Features.self_driving;
                        break;
                    default: continue;
                }
                serviceFeaturesList.Add(serviceFeaturesModel);
            }
            return serviceFeaturesList;
        }
    }
}
