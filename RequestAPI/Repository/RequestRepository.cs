using InternalAPI;
using RideSaver.Server.Models;

namespace RequestAPI.Repository
{
    public class RequestRepository : IRequestRepository
    {
        public readonly IClientRepository _clientRepository;
        private readonly InternalAPI.Services.ServicesClient _servicesClient;
        private readonly ILogger<RequestRepository> _logger;
        public RequestRepository(IClientRepository clientRepository, InternalAPI.Services.ServicesClient servicesClient, ILogger<RequestRepository> logger)
        {
            _clientRepository = clientRepository;
            _servicesClient = servicesClient;
            _logger = logger;
        }
        private async Task<Requests.RequestsClient> getClient(Guid rideId, string token)
        {
            var service = await _servicesClient.GetServiceByHashAsync(new GetServiceByHashRequest { Hash = Google.Protobuf.ByteString.CopyFrom(rideId.ToByteArray(), 0, 4) });
            if (service.Name is null) { throw new ArgumentNullException(nameof(service.Name)); }

            return _clientRepository.GetClientByName(service.ClientId, token);
        }

        public async Task<Ride> GetRideRequestAsync(Guid rideId, string token)
        {
            var rideReplyModel = await (await getClient(rideId, token)).GetRideRequestAsync(new GetRideRequestModel() { RideId = rideId.ToString(), });
            if(rideReplyModel is null) { throw new ArgumentNullException(nameof(rideReplyModel)); }
                 
            return new Ride()
            {
                Id = Guid.Parse(rideReplyModel.RideId),
                EstimatedTimeOfArrival = rideReplyModel.EstimatedTimeOfArrival.ToDateTime(),
                RiderOnBoard = rideReplyModel.RiderOnBoard,
                Stage = (Ride.StageEnum)rideReplyModel.RideStage,
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
                    Height = (decimal)rideReplyModel.DriverLocation.Height
                },
            };
        }
        public async Task<Ride> CreateRideRequestAsync(Guid rideId, string token)
        {
            var rideReplyModel = await (await getClient(rideId, token)).PostRideRequestAsync(new PostRideRequestModel() { EstimateId = rideId.ToString(), });
            if (rideReplyModel is null) { throw new ArgumentNullException(nameof(rideReplyModel)); }

            return new Ride()
            {
                Id = new Guid(rideReplyModel.RideId),
                EstimatedTimeOfArrival = rideReplyModel.EstimatedTimeOfArrival.ToDateTime(),
                RiderOnBoard = rideReplyModel.RiderOnBoard,
                Stage = (Ride.StageEnum)rideReplyModel.RideStage,
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
                }, 
            };
        }
        public async Task<PriceWithCurrency> CancelRideRequestAsync(Guid rideId, string token)
        {
            var rideReplyModel = await (await getClient(rideId, token)).DeleteRideRequestAsync(new DeleteRideRequestModel() { RideId = rideId.ToString(), });
            if (rideReplyModel is null) { throw new ArgumentNullException(nameof(rideReplyModel)); }

            return new PriceWithCurrency()
            {
                Price = (decimal)rideReplyModel.Price,
                Currency = rideReplyModel.Currency
            };
        }
    }
}
