using Grpc.Net.Client;
using RideSaver.Server.Models;
using Grpc.Core;
using Google.Protobuf.Collections;

namespace EstimateAPI.Repository
{
    public class EstimateRepository : IEstimateRepository
    {
        private string LyftChannel = "";
        private string UberChannel = "";
        public async Task<List<Estimate>> GetRideEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats)
        {
            List<Estimate> lyftEstimates = await GetLyftEstimatesAsync(startPoint, endPoint, services, seats);
            List<Estimate> uberEstimates = await GetUberEstimatesAsync(startPoint, endPoint, services, seats);
            return lyftEstimates.Concat(uberEstimates).ToList();
        }
        public async Task<List<Estimate>> GetLyftEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats) // TBA
        {
            var estimatesList = new List<Estimate>();

            var channel = GrpcChannel.ForAddress(LyftChannel);
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
                    Price = estimatesReply.Price,
                    Distance = estimatesReply.Distance,
                    Waypoints = ConvertLocationModelToLocation(estimatesReply.WayPoints), // TBA: IMPLEMENT "LOCATIONMODEL" TO "LOCATION" CONVERTER.
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

            var channel = GrpcChannel.ForAddress(UberChannel);
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
                    Price = estimatesReply.Price,
                    Distance = estimatesReply.Distance,
                    Waypoints = ConvertLocationModelToLocation(estimatesReply.WayPoints), // TBA: IMPLEMENT "LOCATIONMODEL" TO "LOCATION" CONVERTER.
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
            List<Estimate> rideEstimatesRefresh = new List<Estimate>();


           foreach(Estimate id in ids)
           {
                if (id is object) // TBA: Distinguish between uber & lyft ride guids.
                {
                    var estimateRefresh = await GetUberRideEstimateRefreshAsync(id);
                    rideEstimatesRefresh.Add(estimateRefresh);
                } else
                {
                    var estimateRefresh = await GetLyftRideEstimateRefreshAsync(id);
                    rideEstimatesRefresh.Add(estimateRefresh);
                }
           }

            return rideEstimatesRefresh;
        }

        public async Task<Estimate> GetLyftRideEstimateRefreshAsync(Estimate estimate_id) // TBA
        {
            var channel = GrpcChannel.ForAddress(LyftChannel);
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
                Price = estimateRefreshReplyModel.Price,
                Distance = estimateRefreshReplyModel.Distance,
                Waypoints = ConvertLocationModelToLocation(estimateRefreshReplyModel.WayPoints), // TBA: IMPLEMENT "LOCATIONMODEL" TO "LOCATION" CONVERTER
                DisplayName = estimateRefreshReplyModel.DisplayName,
                Seats = estimateRefreshReplyModel.Seats,
                RequestURL = estimateRefreshReplyModel.RequestUrl
            };

            return LyftEstimate;
        }

        public async Task<Estimate> GetUberRideEstimateRefreshAsync(Estimate estimate_id) // TBA
        {
            var channel = GrpcChannel.ForAddress(UberChannel);
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
                Price = estimateRefreshReplyModel.Price,
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
            if (field == null) return new List<Location>();
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
