using Grpc.Net.Client;
using RideSaver.Server.Models;
using Google.Protobuf.Collections;
using InternalAPI;
using Grpc.Core;

namespace EstimateAPI.Repository
{
    public class EstimateRepository : IEstimateRepository
    {
        private readonly string lyftChannel = ""; // Lyft-Service URL
        private readonly string uberChannel = ""; // Uber-Service URL
        public async Task<List<Estimate>> GetRideEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats)
        {
            List<Estimate> lyftEstimates = await GetLyftEstimatesAsync(startPoint, endPoint, services, seats);
            List<Estimate> uberEstimates = await GetUberEstimatesAsync(startPoint, endPoint, services, seats);
            return lyftEstimates.Concat(uberEstimates).ToList();
        }
        public async Task<List<Estimate>> GetLyftEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats) // TBA
        {
            var estimatesList = new List<Estimate>();

            using var channel = GrpcChannel.ForAddress(lyftChannel);
            var estimatesClient = new Estimates.EstimatesClient(channel);
            var clientRequested = new GetEstimatesRequest()
            {
                StartPoint = new LocationModel()
                {
                    Latitude = (double)startPoint.Latitude,
                    Longitude = (double)startPoint.Longitude,
                    Height = (double)startPoint.Height,
                    Planet = startPoint.Planet
                },

                EndPoint = new LocationModel()
                {
                    Latitude = (double)endPoint.Latitude,
                    Longitude = (double)endPoint.Longitude,
                    Height = (double)endPoint.Height,
                    Planet = endPoint.Planet
                },

                Services = { services.ToString() },
                Seats = (int)(seats > 0 ? seats : 0),
            };

            var estimatesReplyModel = estimatesClient.GetEstimates(clientRequested);
            await foreach(var estimatesReply in estimatesReplyModel.ResponseStream.ReadAllAsync())
            {
                await Task.Delay(1000);
                var estimate = new Estimate()
                {
                    Id = new Guid(estimatesReply.EstimateId), // TBA: IMPLEMENT EXCEPTION HANDLING

                    Price = new PriceWithCurrency()
                    {
                        Price = (decimal)estimatesReply.PriceDetails.Price,
                        Currency = estimatesReply.PriceDetails.Currency
                    },

                    Distance = estimatesReply.Distance,
                    Waypoints = ConvertLocationModelToLocation(estimatesReply.WayPoints), 
                    DisplayName = estimatesReply.DisplayName,
                    Seats = estimatesReply.Seats,
                    RequestURL = estimatesReply.RequestUrl,
                    InvalidTime = estimatesReply.CreatedTime.ToDateTime()
                };

                estimatesList.Add(estimate);
            }

            return estimatesList;
        }
        public async Task<List<Estimate>> GetUberEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats) // TBA
        {
            var estimatesList = new List<Estimate>();

            var channel = GrpcChannel.ForAddress(uberChannel);
            var estimatesClient = new Estimates.EstimatesClient(channel);
            var clientRequested = new GetEstimatesRequest()
            {
                StartPoint = new LocationModel()
                {
                    Latitude = (double)startPoint.Latitude,
                    Longitude = (double)startPoint.Longitude,
                    Height = (double)startPoint.Height,
                    Planet = startPoint.Planet
                },

                EndPoint = new LocationModel()
                {
                    Latitude = (double)endPoint.Latitude,
                    Longitude = (double)endPoint.Longitude,
                    Height = (double)endPoint.Height,
                    Planet = endPoint.Planet
                },

                Services = { services.ToString() },
                Seats = (int)(seats > 0 ? seats : 0),
            };

            var estimatesReplyModel = estimatesClient.GetEstimates(clientRequested);
            await foreach (var estimatesReply in estimatesReplyModel.ResponseStream.ReadAllAsync())
            {
                await Task.Delay(1000);
                var estimate = new Estimate()
                {
                    Id = new Guid(estimatesReply.EstimateId), // TBA: IMPLEMENT EXCEPTION HANDLING
          
                    Price = new PriceWithCurrency()
                    { 
                        Price = (decimal)estimatesReply.PriceDetails.Price,
                        Currency = estimatesReply.PriceDetails.Currency
                    },

                    Distance = estimatesReply.Distance,
                    Waypoints = ConvertLocationModelToLocation(estimatesReply.WayPoints),
                    DisplayName = estimatesReply.DisplayName,
                    Seats = estimatesReply.Seats,
                    RequestURL = estimatesReply.RequestUrl,
                    InvalidTime = estimatesReply.CreatedTime.ToDateTime()
                };

                estimatesList.Add(estimate);
            }

            return estimatesList;
        }

        public async Task<List<Estimate>> GetRideEstimatesRefreshAsync(List<object> ids) // TBA
        {
           var rideEstimatesRefresh = new List<Estimate>();
           foreach(var id in ids)
           {
                if (id is not null) // TBA: Distinguish between uber & lyft ride guids.
                {
                    var estimateRefresh = await GetUberRideEstimateRefreshAsync((Guid)id);
                    rideEstimatesRefresh.Add(estimateRefresh);
                } else
                {
                    var estimateRefresh = await GetLyftRideEstimateRefreshAsync((Guid)id);
                    rideEstimatesRefresh.Add(estimateRefresh);
                }
           }

            return rideEstimatesRefresh;
        }

        public async Task<Estimate> GetLyftRideEstimateRefreshAsync(Guid estimate_id)
        {
            var channel = GrpcChannel.ForAddress(lyftChannel);
            var estimatesRefreshClient = new Estimates.EstimatesClient(channel);
            var clientRequested = new GetEstimateRefreshRequest()
            {
                EstimateId = estimate_id.ToString()
            };

            var estimateRefreshReplyModel = await estimatesRefreshClient.GetEstimateRefreshAsync(clientRequested);
            var LyftEstimate = new Estimate()
            {
                Id = new Guid(estimateRefreshReplyModel.EstimateId), // TBA: IMPLEMENT EXCEPTION HANDLING
                InvalidTime = estimateRefreshReplyModel.CreatedTime.ToDateTime(),

                Price = new PriceWithCurrency()
                {
                    Price = (decimal)estimateRefreshReplyModel.PriceDetails.Price,
                    Currency = estimateRefreshReplyModel.PriceDetails.Currency
                },

                Distance = estimateRefreshReplyModel.Distance,
                Waypoints = ConvertLocationModelToLocation(estimateRefreshReplyModel.WayPoints),
                DisplayName = estimateRefreshReplyModel.DisplayName,
                Seats = estimateRefreshReplyModel.Seats,
                RequestURL = estimateRefreshReplyModel.RequestUrl
            };

            return LyftEstimate;
        }

        public async Task<Estimate> GetUberRideEstimateRefreshAsync(Guid estimate_id)
        {
            var channel = GrpcChannel.ForAddress(uberChannel);
            var estimatesRefreshClient = new Estimates.EstimatesClient(channel);
            var clientRequested = new GetEstimateRefreshRequest()
            {
                EstimateId = estimate_id.ToString()
            };

            var estimateRefreshReplyModel = await estimatesRefreshClient.GetEstimateRefreshAsync(clientRequested);
            var UberEstimate = new Estimate()
            {
                Id = new Guid(estimateRefreshReplyModel.EstimateId), // TBA: IMPLEMENT EXCEPTION HANDLING
                InvalidTime = estimateRefreshReplyModel.CreatedTime.ToDateTime(),

                Price = new PriceWithCurrency()
                {
                    Price = (decimal)estimateRefreshReplyModel.PriceDetails.Price,
                    Currency = estimateRefreshReplyModel.PriceDetails.Currency
                },

                Distance = estimateRefreshReplyModel.Distance,
                Waypoints = ConvertLocationModelToLocation(estimateRefreshReplyModel.WayPoints),
                DisplayName = estimateRefreshReplyModel.DisplayName,
                Seats = estimateRefreshReplyModel.Seats,
                RequestURL = estimateRefreshReplyModel.RequestUrl
            };

            return UberEstimate;
        }

        public List<Location> ConvertLocationModelToLocation(RepeatedField<LocationModel> field) // Converts RepeatedField<LocationModel> to List<Location>
        {
            if (field is null) return new List<Location>();
            var fieldList = field.ToList();
            var locationList = new List<Location>();
            foreach(var f in fieldList)
            {
                var location = new Location()
                {
                    Latitude = (decimal)f.Latitude,
                    Longitude = (decimal)f.Longitude,
                    Height = (decimal)f.Height,
                    Planet = f.Planet,
                };

                locationList.Add(location);
;            }
            return locationList;
        }
    }
}
