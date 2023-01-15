using DataAccess.DataModels;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using InternalAPI;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.Data;
using Grpc.Core;
using MySqlConnector;

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
            var estimateHash = BitConverter.ToString(request.Hash.ToByteArray()).Replace("-", string.Empty).ToLower();
            ServicesModel? service = await _serviceContext.Services!.FromSqlRaw("SELECT * FROM services WHERE @Id = SUBSTRING(SHA1(Id), 1, 8)", new MySqlParameter("@Id", estimateHash)).FirstOrDefaultAsync();

            if(service is null) {  throw new ArgumentNullException(nameof(service)); }

            ServicesModel servicesModel = new()
            {
                Id = service.Id,
                Name = service.Name!.ToString(),
                ClientId = service.ClientId!.ToString(),
                CreatedAt = service.CreatedAt,
            };

            ServiceModel serviceModel = new()
            {
                Name = servicesModel.Name.ToString(),
                ClientId = servicesModel.ClientId!.ToString(),
            };

            return serviceModel;
        }
        public override async Task GetServices(Empty request, IServerStreamWriter<ServiceModel> responseStream, ServerCallContext context)
        {
            IList<ServicesModel> services = (IList<ServicesModel>)_serviceContext.Services!.ToListAsync();
            foreach (var service in services)
            {
                await responseStream.WriteAsync(new ServiceModel
                {
                    Name = service.Name,
                    ClientId = service.ClientId,
                });
            }
        }
        public override async Task<InternalAPI.Void> RegisterService(RegisterServiceRequest request, ServerCallContext context)
        {
            var service = new ServicesModel()
            {
                Id = new Guid(request.Id.ToByteArray()),
                Name = request.Name,
                ClientId = request.ClientName,
                ServiceFeatures = ConvertServiceFeaturesToServiceFeaturesModel(request.Features)
            };

            if (!_serviceContext.Services!.Contains(service))
            {
                await _serviceContext.Services!.AddAsync(service);
                await _serviceContext.SaveChangesAsync();
            }              
            return await Task.FromResult(new InternalAPI.Void());
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
