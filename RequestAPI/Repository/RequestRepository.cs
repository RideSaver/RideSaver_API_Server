using InternalAPI;
using RideSaver.Server.Models;

namespace RequestAPI.Repository
{
    public class RequestRepository : IRequestRepository
    {
        public readonly IClientRepository _clientRepository;
        private readonly Services.ServicesClient _servicesClient;
        public RequestRepository(IClientRepository clientRepository, Services.ServicesClient servicesClient)
        {
            _clientRepository = clientRepository;
            _servicesClient = servicesClient;
        }
        private async Task<Requests.RequestsClient> getClient(Guid rideId)
        {
            var service = await _servicesClient.GetServiceByHashAsync(new GetServiceByHashRequest
            {
                Hash = Google.Protobuf.ByteString.CopyFrom(rideId.ToByteArray(), 0, 4)
            });

            if (service.Name == null)
            {
                throw new NotImplementedException();
            }

            if (!_clientRepository.Clients.ContainsKey(service.Name))
            {
                // Refresh cache, in case the service came up during the 10s cache window
                await _clientRepository.RefreshClients();
                if (!_clientRepository.Clients.ContainsKey(service.Name))
                {
                    throw new NotImplementedException(); // Should never happen, SQL has a service that does not exist in K8s
                }
            }

            return _clientRepository.Clients[service.Name];
        }
        public async Task<Ride> GetRideRequestAsync(Guid rideId)
        {
            var rideReplyModel = await (await getClient(rideId)).GetRideRequestAsync(new GetRideRequestModel()
            {
                RideId = rideId.ToString(),
            });

            return new Ride()
            {
                Id = new Guid(rideReplyModel.RideId),
                EstimatedTimeOfArrival = rideReplyModel.EstimatedTimeOfArrival.ToDateTime(),
                RiderOnBoard = rideReplyModel.RiderOnBoard,

                Price = new PriceWithCurrency()
                {
                    Price = (decimal)rideReplyModel.Price.Price,
                    Currency = rideReplyModel.Price.Currency
                },

                Driver = new Driver()
                {
                    DisplayName = rideReplyModel.Driver.DisplayName,
                    LicensePlate = rideReplyModel.Driver.LicensePlate,
                    CarPicture = rideReplyModel.Driver.CarPicture,
                    CarDescription = rideReplyModel.Driver.CarDescription,
                    DriverPronounciation = rideReplyModel.Driver.DriverPronounciation,
                },

                DriverLocation = new Location()
                {
                    Latitude = (decimal)rideReplyModel.DriverLocation.Latitude,
                    Longitude = (decimal)rideReplyModel.DriverLocation.Longitude,
                    Height = (decimal)rideReplyModel.DriverLocation.Height,
                    Planet = rideReplyModel.DriverLocation.Planet,
                },

                Stage = (Ride.StageEnum)rideReplyModel.RideStage
            };
        }
        public async Task<Ride> CreateRideRequestAsync(Guid rideId)
        {
            var rideReplyModel = await (await getClient(rideId)).PostRideRequestAsync(new PostRideRequestModel()
            {
                EstimateId = rideId.ToString(),
            });

            return new Ride()
            {
                Id = new Guid(rideReplyModel.RideId),
                EstimatedTimeOfArrival = rideReplyModel.EstimatedTimeOfArrival.ToDateTime(),
                RiderOnBoard = rideReplyModel.RiderOnBoard,

                Price = new PriceWithCurrency()
                {
                    Price = (decimal)rideReplyModel.Price.Price,
                    Currency = rideReplyModel.Price.Currency
                },

                Driver = new Driver()
                {
                    DisplayName = rideReplyModel.Driver.DisplayName,
                    LicensePlate = rideReplyModel.Driver.LicensePlate,
                    CarPicture = rideReplyModel.Driver.CarPicture,
                    CarDescription = rideReplyModel.Driver.CarDescription,
                    DriverPronounciation = rideReplyModel.Driver.DriverPronounciation,
                },

                DriverLocation = new Location()
                {
                    Latitude = (decimal)rideReplyModel.DriverLocation.Latitude,
                    Longitude = (decimal)rideReplyModel.DriverLocation.Longitude,
                    Height = (decimal)rideReplyModel.DriverLocation.Height,
                    Planet = rideReplyModel.DriverLocation.Planet,
                },

                Stage = (Ride.StageEnum)rideReplyModel.RideStage,
            };
        }
        public async Task<PriceWithCurrency> CancelRideRequestAsync(Guid rideId)
        {
            var rideReplyModel = await (await getClient(rideId)).DeleteRideRequestAsync(new DeleteRideRequestModel()
            {
                RideId = rideId.ToString(),
            });

            return new PriceWithCurrency()
            {
                Price = (decimal)rideReplyModel.Price,
                Currency = rideReplyModel.Currency
            };
        }
    }
}
