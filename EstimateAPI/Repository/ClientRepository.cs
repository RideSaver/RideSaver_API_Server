using EstimateAPI.Configuration;
using Grpc.Net.Client;
using InternalAPI;
using k8s;
using Microsoft.Extensions.Options;

namespace EstimateAPI.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientDiscoveryOptions _options;
        private readonly ILogger<EstimateController> _logger;

        private readonly IKubernetes _kubernetes;
        private readonly string _labelStr;
        private string _namespace;

        public ClientRepository(IOptions<ClientDiscoveryOptions> options, ILogger<ClientRepository> logger) : this(options.Value.Namespace, options, logger) { }

        public ClientRepository(string Namespace, IOptions<ClientDiscoveryOptions> options, ILogger<ClientRepository> logger)
        {
            _namespace = Namespace;
            _options = options.Value;
            _logger = logger;

            // Get the Kubernetes Object
            var config = KubernetesClientConfiguration.InClusterConfig();
            _logger.LogDebug($"kubernetes configuration: {config}");
            _kubernetes = new Kubernetes(config);

            // Construct the label filter
            List<string> labelStrs = new();
            foreach (var option in _options.Labels)
            {
                labelStrs.Add($"{option.Key}={option.Value}");
            }

            _labelStr = string.Join(",", labelStrs.ToArray());
            _logger.LogDebug($"kubernetes label string: {_labelStr}");
        }

        public Estimates.EstimatesClient GetClientByName(string name)
        {
            _logger.LogDebug($"Requesting client for name: {name}");
            GrpcChannel channel = GrpcChannel.ForAddress($"https://{name}.client");
            return new Estimates.EstimatesClient(channel);
        }

        public async Task<List<Estimates.EstimatesClient>> GetClients()
        {
            _logger.LogDebug($"Getting kubernetes clients");
            var list = await _kubernetes.CoreV1.ListNamespacedServiceAsync(_namespace, labelSelector: _labelStr);
            _logger.LogDebug($"Received clients: {list}");
            List<Estimates.EstimatesClient> Clients = new List<Estimates.EstimatesClient>();
            foreach (var client in list)
            {
                _logger.LogDebug($"kubernetes client: {client.Metadata.Name}");
                Clients.Add(GetClientByName(client.Metadata.Name));
            }
            return Clients;
        }
    }
}
