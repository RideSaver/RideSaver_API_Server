using InternalAPI;
using RideSaver.Server.Models;
using System.Net.Http.Headers;

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
            var service = await _servicesClient.GetServiceByHashAsync(new GetServiceByHashRequest
            {
                Hash = Google.Protobuf.ByteString.CopyFrom(rideId.ToByteArray())
            });

            if (service.Name == null)
            {
                _logger.LogError("[RequestAPI::RequestRepository::getClient] Failed to find a service-name to retrieve the client name!");
                throw new NotImplementedException();
            }

            _logger.LogInformation($"[RequestAPI::RequestRepository::getClient] Service name succesfully found: {service.ClientId} ! Matching client names..");
            return _clientRepository.GetClientByName(service.ClientId, token);
        }

        public async Task<Ride> GetRideRequestAsync(Guid rideId, string token)
        {
            var rideReplyModel = await (await getClient(rideId, token)).GetRideRequestAsync(new GetRideRequestModel()
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
                    Height = (decimal)rideReplyModel.DriverLocation.Height
                },

                Stage = (Ride.StageEnum)rideReplyModel.RideStage
            };
        }
        public async Task<Ride> CreateRideRequestAsync(Guid rideId, string token)
        {
            var rideReplyModel = await (await getClient(rideId, token)).PostRideRequestAsync(new PostRideRequestModel()
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
                },

                Stage = (Ride.StageEnum)rideReplyModel.RideStage,
            };
        }
        public async Task<PriceWithCurrency> CancelRideRequestAsync(Guid rideId, string token)
        {
            var rideReplyModel = await (await getClient(rideId, token)).DeleteRideRequestAsync(new DeleteRideRequestModel()
            {
                RideId = rideId.ToString(),
            });

            return new PriceWithCurrency()
            {
                Price = (decimal)rideReplyModel.Price,
                Currency = rideReplyModel.Currency
            };
        }

        public string GetAuthorizationToken(Microsoft.Extensions.Primitives.StringValues headers)
        {
            string? token = null;
            if (AuthenticationHeaderValue.TryParse(headers, out var headerValue))
            {
                token = headerValue.Parameter;
            }

            return token;
        }
    }
}
