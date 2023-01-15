using Grpc.Core;
using InternalAPI;
using RideSaver.Server.Models;
using Google.Protobuf.Collections;

namespace EstimateAPI.Repository
{
    public class EstimateRepository : IEstimateRepository
    {
        public readonly IClientRepository _clientRepository;
        private readonly ILogger<EstimateRepository> _logger;
        private readonly InternalAPI.Services.ServicesClient _servicesClient;
        
        public EstimateRepository(IClientRepository clientRepository, InternalAPI.Services.ServicesClient servicesClient, ILogger<EstimateRepository> logger)
        {
            _clientRepository = clientRepository;
            _servicesClient = servicesClient;
            _logger = logger;
        }

        public async Task<List<Estimate>> GetRideEstimatesAsync(Location startPoint, Location endPoint, List<Guid> services, int? seats, string token)
        {
            IEnumerable<Task<List<Estimate>>> estimateTasksQuery =
                from client in await _clientRepository.GetClients(token)
                select GetEstimatesAsync(client, startPoint, endPoint, services, seats);

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
            // Initalize a new list Estimates to queery the data recieved in the request for different services.
            var estimatesList = new List<Estimate>();
            var clientRequested = new GetEstimatesRequest()
            {
                StartPoint = new LocationModel()
                {
                    Latitude = (double)startPoint.Latitude,
                    Longitude = (double)startPoint.Longitude,
                    Height = (double)startPoint.Height
                },
                EndPoint = new LocationModel()
                {
                    Latitude = (double)endPoint.Latitude,
                    Longitude = (double)endPoint.Longitude,
                    Height = (double)endPoint.Height,
                },
                Seats = (int)(seats > 0 ? seats : 0),
            };

            // Throw an exception if the list of ServiceIDs recieved is null
            if (services is null) { throw new ArgumentNullException(nameof(services)); }

            // Add each ServiceID to the client-services to match the service-name for each service.
            foreach (var service in services!) { clientRequested.Services.Add(service.ToString()); }

            // Make the request to the different clients & iterate through the response while creating new Estimates for each.
            using var estimatesReplyModel = client.GetEstimates(clientRequested);
            await foreach (var estimatesReply in estimatesReplyModel.ResponseStream.ReadAllAsync())
            {
                var estimate = new Estimate()
                {
                    Id = Guid.Parse(estimatesReply.EstimateId),
                    Distance = estimatesReply.Distance,
                    Waypoints = ConvertLocationModelToLocation(estimatesReply.WayPoints),
                    DisplayName = estimatesReply.DisplayName,
                    Seats = estimatesReply.Seats,
                    RequestURL = estimatesReply.RequestUrl,
                    InvalidTime = estimatesReply.CreatedTime.ToDateTime(),
                    Price = new PriceWithCurrency()
                    {
                        Price = (decimal)estimatesReply.PriceDetails.Price,
                        Currency = estimatesReply.PriceDetails.Currency
                    },
                };
                estimatesList.Add(estimate);
            }
            return estimatesList;
        }

        public async Task<List<Estimate>> GetRideEstimatesRefreshAsync(List<Guid> ids, string token) // TBA
        {
            List<Estimate> estimates = new();
            List<Task<Estimate>> rideEstimatesRefreshTasks = new();
            foreach (var id in ids)
            {
                var service = await _servicesClient.GetServiceByHashAsync(new GetServiceByHashRequest { Hash = Google.Protobuf.ByteString.CopyFrom(id.ToByteArray(), 0, 4) });
                rideEstimatesRefreshTasks.Add(GetRideEstimateRefreshAsync(_clientRepository.GetClientByName(service.ClientId, token), id));
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
            var clientRequested = new GetEstimateRefreshRequest() { EstimateId = estimate_id.ToString() };
            var estimateRefreshReplyModel = await client.GetEstimateRefreshAsync(clientRequested);
            var estimate = new Estimate()
            {
                Id = new Guid(estimateRefreshReplyModel.EstimateId),
                InvalidTime = estimateRefreshReplyModel.CreatedTime.ToDateTime(),
                Distance = estimateRefreshReplyModel.Distance,
                Waypoints = ConvertLocationModelToLocation(estimateRefreshReplyModel.WayPoints),
                DisplayName = estimateRefreshReplyModel.DisplayName,
                Seats = estimateRefreshReplyModel.Seats,
                RequestURL = estimateRefreshReplyModel.RequestUrl,
                Price = new PriceWithCurrency()
                {
                    Price = (decimal)estimateRefreshReplyModel.PriceDetails.Price,
                    Currency = estimateRefreshReplyModel.PriceDetails.Currency
                },
            };
            return estimate;
        }

        public List<Location> ConvertLocationModelToLocation(RepeatedField<LocationModel> field) // Converts RepeatedField<LocationModel> to List<Location>
        {
            if (field is null) return new List<Location>();
            var fieldList = field.ToList();
            var locationList = new List<Location>();

            foreach (var f in fieldList)
            {
                var location = new Location()
                {
                    Latitude = (decimal)f.Latitude,
                    Longitude = (decimal)f.Longitude,
                    Height = (decimal)f.Height,
                };
                locationList.Add(location);
            }
            return locationList;
        }
    }
}
