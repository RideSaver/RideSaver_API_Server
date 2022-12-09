using Grpc.Net.Client;
using RideSaver.Server.Models;
using Google.Protobuf.Collections;
using InternalAPI;
using Grpc.Core;

namespace EstimateAPI.Repository
{
    public class EstimateRepository : IEstimateRepository
    {
        public readonly IClientRepository _clientRepository;

        public EstimateRepository(IClientRepository clientRepository) => _clientRepository = clientRepository;

        public async Task<List<Estimate>> GetRideEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats)
        {
            IEnumerable<Task<List<Estimate>>> estimateTasksQuery =
                from client in _clientRepository.Clients
                select GetEstimatesAsync(client.Value, startPoint, endPoint, services, seats);
            List<Task<List<Estimate>>> estimateTasks = estimateTasksQuery.ToList();
            List<Estimate> estimates = new List<Estimate>();

            while (estimateTasks.Any())
            {
                Task<List<Estimate>> finishedTask = await Task.WhenAny(estimateTasks);
                estimateTasks.Remove(finishedTask);
                estimates.AddRange(await finishedTask);
            }
            return estimates;
        }

        public async Task<List<Estimate>> GetEstimatesAsync(Estimates.EstimatesClient client, Location startPoint, Location endPoint, List<Guid> services, int? seats) // TBA
        {
            var estimatesList = new List<Estimate>();
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

            var estimatesReplyModel = client.GetEstimates(clientRequested);
            await foreach(var estimatesReply in estimatesReplyModel.ResponseStream.ReadAllAsync())
            {
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

        public async Task<List<Estimate>> GetRideEstimatesRefreshAsync(List<Guid> ids) // TBA
        {
            List<Estimate> estimates = new List<Estimate>();
            List<Task<Estimate>> rideEstimatesRefreshTasks = new List<Task<Estimate>>();
            var servicesClient = new Services.ServicesClient(GrpcChannel.ForAddress($"https://services.api"));
            foreach(var id in ids)
            {
                var service = await servicesClient.GetServiceByHashAsync(new GetServiceByHashRequest {
                    Hash = Google.Protobuf.ByteString.CopyFrom(id.ToByteArray(), 0, 4)
                });
                rideEstimatesRefreshTasks.Add(GetRideEstimateRefreshAsync(_clientRepository.Clients[service.ClientId], id));
            }

            while (rideEstimatesRefreshTasks.Any())
            {
                Task<Estimate> finishedTask = await Task.WhenAny(rideEstimatesRefreshTasks);
                rideEstimatesRefreshTasks.Remove(finishedTask);
                estimates.Add(await finishedTask);
            }

            return estimates;
        }

        public async Task<Estimate> GetRideEstimateRefreshAsync(Estimates.EstimatesClient client, Guid estimate_id)
        {
            var clientRequested = new GetEstimateRefreshRequest()
            {
                EstimateId = estimate_id.ToString()
            };

            var estimateRefreshReplyModel = await client.GetEstimateRefreshAsync(clientRequested);
            var estimate = new Estimate()
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

            return estimate;
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
