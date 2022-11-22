using RideSaver.Server.Models;

namespace ServicesAPI.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        public RideService GetService(Guid service_id) // TBA
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RideService>> GetAvailableServices() // TBA:-> Will pull the service data from the DB.
        {
            List<RideService> rideServices = new List<RideService>() {
                new RideService {
                    DisplayName = "lyftPool",
                    Id = new Guid(),
                    Provider = new Guid(),
                },
                 new RideService {
                    DisplayName = "uberPool",
                    Id = new Guid(),
                    Provider = new Guid(),

                },
                  new RideService {
                    DisplayName = "uber",
                    Id = new Guid(),
                    Provider = new Guid(),
                },
                   new RideService {
                    DisplayName = "lyft",
                    Id = new Guid(),
                    Provider = new Guid(),
                },
            };

            await Task.Delay(1000);
            return rideServices;
        }
    }
}
