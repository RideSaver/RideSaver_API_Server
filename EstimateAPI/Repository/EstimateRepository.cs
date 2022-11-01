using Grpc.Net.Client;
using RideSaver.Server.Models;

using Grpc.Core.Api;
using Grpc.Core;

namespace EstimateAPI.Repository
{
    public class EstimateRepository : IEstimateRepository
    {
        private string LyftChannel = "";
        private string UberChannel = "";
        public async Task<List<Estimate>> GetRideEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats)
        {
            List<Estimate> lyftEstimates = await GetLyftEstimates(startPoint, endPoint, services, seats);
            List<Estimate> uberEstimates = await GetUberEstimates(startPoint, endPoint, services, seats);
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
                estimatesList.Add(estimatesReply);
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
                estimatesList.Add(estimatesReply);
            }

            return estimatesList;
        }

        public List<Estimate> GetRideEstimatesRefresh(List<object> ids) // TBA
        {
            List<Estimate> rideEstimatesRefresh = new List<Estimate>();


           foreach(Estimate id in ids)
           {
                if (id is object) // TBA: Distinguish between uber & lyft ride guids.
                {
                    var estimateRefresh = GetUberRideEstimateRefresh(id);
                    rideEstimatesRefresh.Add(estimateRefresh);
                } else
                {
                    var estimateRefresh = GetLyftRideEstimateRefresh(id);
                    rideEstimatesRefresh.Add(estimateRefresh);
                }
           }

            return rideEstimatesRefresh;
        }

        public Estimate GetLyftRideEstimateRefresh(Estimate estimate_id) // TBA
        {
            var channel = GrpcChannel.ForAddress(LyftChannel);
            var estimatesRefreshClient = new Estimates.EstimatesClient(channel);
            var clientRequested = new GetEstimateRefreshRequest()
            {
                EstimateId = estimate_id
            };

            var estimateRefreshReplyModel = estimatesRefreshClient.GetEstimateRefresh(clientRequested);
            var LyftEstimate = new Estimate()
            {
                Id = estimateRefreshReplyModel.EstimateId,
                InvalidTime = estimateRefreshReplyModel.CreatedTime,
                Price = estimateRefreshReplyModel.Price,
                Distance = estimateRefreshReplyModel.Distance,
                Waypoints = estimateRefreshReplyModel.WayPoints.ToList(),
                DisplayName = estimateRefreshReplyModel.DisplayName,
                Seats = estimateRefreshReplyModel.Seats,
                RequestURL = estimateRefreshReplyModel.RequestUrl
            };

            return LyftEstimate;
        }

        public Estimate GetUberRideEstimateRefresh(Estimate estimate_id) // TBA
        {
            var channel = GrpcChannel.ForAddress(UberChannel);
            var estimatesRefreshClient = new Estimates.EstimatesClient(channel);
            var clientRequested = new GetEstimateRefreshRequest()
            {
                EstimateId = estimate_id
            };

            var estimateRefreshReplyModel = estimatesRefreshClient.GetEstimateRefresh(clientRequested);
            var UberEstimate = new Estimate()
            {
                Id = estimateRefreshReplyModel.EstimateId,
                InvalidTime = estimateRefreshReplyModel.CreatedTime,
                Price = estimateRefreshReplyModel.Price,
                Distance = estimateRefreshReplyModel.Distance,
                Waypoints = estimateRefreshReplyModel.WayPoints.ToList(),
                DisplayName = estimateRefreshReplyModel.DisplayName,
                Seats = estimateRefreshReplyModel.Seats,
                RequestURL = estimateRefreshReplyModel.RequestUrl
            };

            return UberEstimate;
        }
    }
}
