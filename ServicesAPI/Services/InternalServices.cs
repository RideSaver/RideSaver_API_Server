using Grpc.Core;
using InternalAPI;
using Google.Protobuf.WellKnownTypes;
using DataAccess.Models;
using ServicesAPI.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using static DataAccess.Models.ServiceFeaturesModel;
using Google.Protobuf.Collections;

namespace ServicesAPI.Services
{
    // Summary: Handles all requests for estimates
    public class InternalServices : InternalAPI.Services.ServicesBase, IInternalServices
    {
        private readonly ServiceContext _serviceContext;
        public InternalServices(ServiceContext serviceContext) => _serviceContext = serviceContext;
        public override async Task<ServiceModel> GetServiceByHash(GetServiceByHashRequest request, ServerCallContext context)
        {
            var EstimateId = new SqlParameter("@EstimateId", request.Hash);
            var service = await _serviceContext.Services.FromSql($"SELECT * FROM services {EstimateId} = SUBSTRING(HASHBYTES('SHA1', Id), 0, 4))").FirstOrDefaultAsync();
            return new ServiceModel
            {
                Name = service?.Name,
                ClientId = service?.ClientId,
            };
        }
        public override async Task GetServices(Empty request, IServerStreamWriter<ServiceModel> responseStream, ServerCallContext context)
        {
            using (var scope = new TransactionScope())
            {
                IList<ServicesModel> services = (IList<ServicesModel>)_serviceContext.Services.ToListAsync();
                foreach (var service in services)
                {
                    await responseStream.WriteAsync(new ServiceModel
                    {
                        Name = service.Name,
                        ClientId = service.ClientId,
                    });
                }
                scope.Complete();
            }
        }
        public override Task<Empty> RegisterService(RegisterServiceRequest request, ServerCallContext context)
        {
            using (var scope = new TransactionScope())
            {
                var service = new ServicesModel()
                {
                    Id = new Guid(request.Id.ToByteArray()),
                    Name = request.Name,
                    ClientId = request.ClientName,
                    ProviderId = new Guid(),
                    ServiceFeatures = ConvertServiceFeaturesToServiceFeaturesModel(request.Features)
                };

                _serviceContext.Services.Add(service);
                foreach (var features in request.Features)
                {
                    var feature = new ServiceFeaturesModel { ServiceId = new Guid(request.Id.ToByteArray()) };
                    switch (features)
                    {
                        case ServiceFeatures.Shared:
                            feature.Feature = Features.shared;
                            break;
                        case ServiceFeatures.Cash:
                            feature.Feature = Features.cash;
                            break;
                        case ServiceFeatures.ProfessionalDriver:
                            feature.Feature = Features.professional_driver;
                            break;
                        case ServiceFeatures.SelfDriving:
                            feature.Feature = Features.self_driving;
                            break;
                        default:
                            continue;
                    }
                    _serviceContext.ServicesFeatures.Add(feature);
                }
                _serviceContext.SaveChanges();
            }

            return (Task<Empty>)Task.CompletedTask;
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
