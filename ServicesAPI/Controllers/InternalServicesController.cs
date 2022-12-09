using Grpc.Core;
using InternalAPI;
using Google.Protobuf.WellKnownTypes;
using DataAccess.Models;
using ServicesAPI.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ServicesAPI.Controllers
{
    // Summary: Handles all requests for estimates
    public class InternalServicesController : Services.ServicesBase
    {
        private readonly ServiceContext _serviceContext;
        public InternalServicesController(ServiceContext serviceContext) => _serviceContext = serviceContext;
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
                foreach(var service in services)
                {
                    await responseStream.WriteAsync(new ServiceModel {
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
                    ProviderId  = new Guid()
                };

                _serviceContext.Services.Add(service);
                foreach(var features in request.Features)
                {
                    var feature = new ServiceFeaturesModel { ServiceId = new Guid(request.Id.ToByteArray()) };
                    switch(features)
                    {
                        case ServiceFeatures.Shared:
                            feature.Feature = ServiceFeaturesModel.Features.shared;
                            break;
                        case ServiceFeatures.Cash:
                            feature.Feature = ServiceFeaturesModel.Features.cash;
                            break;
                        case ServiceFeatures.ProfessionalDriver:
                            feature.Feature = ServiceFeaturesModel.Features.professional_driver;
                            break;
                        case ServiceFeatures.SelfDriving:
                            feature.Feature = ServiceFeaturesModel.Features.self_driving;
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
    }
}
