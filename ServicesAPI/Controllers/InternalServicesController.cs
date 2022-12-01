using Grpc.Core;
using InternalAPI;
using ServicesAPI.Repository;
using Google.Protobuf.WellKnownTypes;
using DataAccess.Models;

namespace ServicesAPI.Controllers
{
    // Summary: Handles all requests for estimates
    public class InternalServicesController : Services.ServicesBase
    {
        Task<ServiceModel> GetServiceByHash(GetServiceByHashRequest request, grpc::ServerCallContext context)
        {
            using (ServiceContext context = new ServiceContext())
            {
                var service = ctx.Services
                    .SqlQuery("SELECT * FROM services @EstimateId = SUBSTRING(HASHBYTES('SHA1', Id), 0, 4))", new SqlParameter("@EstimateId", request.hash));
                    .FirstOrDefault();
                return new ServiceModel {
                    Name = service.Name,
                    ClientId = service.ClientId
                };
            }
        }
        Task GetServices(Empty request, IServerStreamWriter<ServiceModel> responseStream, ServerCallContext context)
        {
            using (ServiceContext context = new ServiceContext())
            {
                IList<ServicesModel> services = ctx.Students.ToList<Student>();
                foreach(var service in services)
                {
                    await responseStream.WriteAsync(new ServiceModel {
                        Name = service.Name;
                        ClientId = service.ClientId
                    });
                }
            }
        }
        Task<Empty> RegisterService(RegisterServiceRequest request, ServerCallContext context)
        {
            using (ServiceContext context = new ServiceContext())
            {

                var service = new ServicesModel()
                {
                    Id = new Guid(request.Id),

                    Name = request.Name

                    ClientId = request.ClientName,

                    ProviderId  = new Guid();
                };
        
                ctx.Services.Add(service);

                foreach(var feature in request.features)
                {
                    var feature = new ServiceFeaturesModel {
                        ServiceId = request.Id
                    };
                    switch(feature)
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
                    ctx.ServicesFeatures.Add(feature);
                }

                ctx.SaveChanges();   
            }
            return new Empty();
        }
    }
}