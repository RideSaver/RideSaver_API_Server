using Grpc.Net.Client;
using Grpc.Core;
using Google.Protobuf.Collections;
using RideSaver.Server.Models;
using Microsoft.AspNetCore.Http.Features;

namespace RequestAPI.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly string lyftChannel = "";
        private readonly string uberChannel = "";
        public async Task<Ride> GetRideRequestIDAsync(Guid ride_id) => ride_id.ToString() == "TBA" ? (await GetLyftRideRequestIDAsync(ride_id)) : (await GetUberRideRequestIDAsync(ride_id));
        public async Task<Ride> PostRideRequestAsync(Guid estimate_id) => estimate_id.ToString() == "TBA" ? (await PostLyftRideRequestAsync(estimate_id)) : (await PostUberRideRequestAsync(estimate_id));
        public async Task<PriceWithCurrency> DeleteRideRequestAsync(Guid ride_id) => ride_id.ToString() == "TBA" ? (await DeleteLyfttRideRequestAsync(ride_id)) : (await DeleteUberRideRequestAsync(ride_id));
        public async Task<Ride> GetLyftRideRequestIDAsync(Guid ride_id)
        {
            var channel = GrpcChannel.ForAddress(lyftChannel);
            var requestsClient = new Requests.RequestsClient(channel);
            var clientRequested = new GetRideRequestModel()
            {
                RideId = ride_id.ToString(),
            };

            var rideReplyModel = await requestsClient.GetRideRequestAsync(clientRequested);
            var lyftRide = new Ride()
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

            return lyftRide;

        }

        public async Task<Ride> GetUberRideRequestIDAsync(Guid ride_id)
        {
            var channel = GrpcChannel.ForAddress(uberChannel);
            var requestsClient = new Requests.RequestsClient(channel);
            var clientRequested = new GetRideRequestModel()
            {
                RideId = ride_id.ToString(),
            };

            var rideReplyModel = await requestsClient.GetRideRequestAsync(clientRequested);
            var uberRide = new Ride()
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

            return uberRide;
        }

        public async Task<Ride> PostUberRideRequestAsync(Guid estimate_id)
        {
            var channel = GrpcChannel.ForAddress(uberChannel);
            var requestsClient = new Requests.RequestsClient(channel);
            var clientRequested = new PostRideRequestModel()
            {
                EstimateId = estimate_id.ToString(),
            };

            var rideReplyModel = await requestsClient.PostRideRequestAsync(clientRequested);
            var uberRide = new Ride()
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

            return uberRide;

        }
        public async Task<Ride> PostLyftRideRequestAsync(Guid estimate_id)
        {
            var channel = GrpcChannel.ForAddress(lyftChannel);
            var requestsClient = new Requests.RequestsClient(channel);
            var clientRequested = new PostRideRequestModel()
            {
                EstimateId = estimate_id.ToString(),
            };

            var rideReplyModel = await requestsClient.PostRideRequestAsync(clientRequested);
            var lyftRide = new Ride()
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

            return lyftRide;
        }

   
        public async Task<PriceWithCurrency> DeleteLyfttRideRequestAsync(Guid ride_id)
        {
            var channel = GrpcChannel.ForAddress(lyftChannel);
            var requestsClient = new Requests.RequestsClient(channel);
            var clientRequested = new DeleteRideRequestModel()
            {
                RideId = ride_id.ToString(),
            };

            var rideReplyModel = await requestsClient.DeleteRideRequestAsync(clientRequested);
            var priceModel = new PriceWithCurrency()
            {
                Price = (decimal)rideReplyModel.Price,
                Currency = rideReplyModel.Currency
            };

            return priceModel;
        }

        public async Task<PriceWithCurrency> DeleteUberRideRequestAsync(Guid ride_id)
        {
            var channel = GrpcChannel.ForAddress(uberChannel);
            var requestsClient = new Requests.RequestsClient(channel);
            var clientRequested = new DeleteRideRequestModel()
            {
                RideId = ride_id.ToString(),
            };

            var rideReplyModel = await requestsClient.DeleteRideRequestAsync(clientRequested);
            var priceModel = new PriceWithCurrency()
            {
                Price = (decimal)rideReplyModel.Price,
                Currency = rideReplyModel.Currency
            };

            return priceModel;
        }
    }
}
