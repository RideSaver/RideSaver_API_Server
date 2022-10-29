using RideSaver.Server.Models;

namespace ServicesAPI.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        public RideService GetService(Guid service_id) // TBA
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RideService> GetAvailableServices() // TBA:-> Will pull the service data from the DB.
        {
            List<RideService> rideServices = new List<RideService>() {
                new RideService {
                    DisplayName = "lyftPool",
                    Id = new Guid(),
                    Security = new AuthorizationMethod()
                    {
                        Authorization = AuthorizationMethod.AuthorizationEnum.NoneEnum
                    },
                },
                 new RideService {
                    DisplayName = "uberPool",
                    Id = new Guid(),
                    Security = new AuthorizationMethod()
                    {
                        Authorization = AuthorizationMethod.AuthorizationEnum.NoneEnum
                    },
                },
                  new RideService {
                    DisplayName = "uber",
                    Id = new Guid(),
                    Security = new AuthorizationMethod()
                    {
                        Authorization = AuthorizationMethod.AuthorizationEnum.NoneEnum
                    },
                },
                   new RideService {
                    DisplayName = "lyft",
                    Id = new Guid(),
                    Security = new AuthorizationMethod()
                    {
                        Authorization = AuthorizationMethod.AuthorizationEnum.NoneEnum
                    },
                },
            };

            return rideServices;
        }
    }
}
