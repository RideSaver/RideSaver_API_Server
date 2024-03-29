using EstimateAPI.Configuration;
using Grpc.Core;
using Grpc.Net.Client;
using InternalAPI;
using k8s;
using Microsoft.Extensions.Options;

namespace EstimateAPI.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientDiscoveryOptions _options;
        private readonly ILogger<ClientRepository> _logger;

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
            _logger.LogDebug($"Kubernetes configuration: {config}");
            _kubernetes = new Kubernetes(config);

            // Construct the label filter
            List<string> labelStrs = new();
            foreach (var option in _options.Labels)
            {
                labelStrs.Add($"{option.Key}={option.Value}");
            }

            _labelStr = string.Join(",", labelStrs.ToArray());
            _logger.LogDebug($"Kubernetes label string: {_labelStr}");
        }

        public Estimates.EstimatesClient GetClientByName(string name, string token)
        {
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                metadata.Add("token", $"{token}");
                return Task.CompletedTask;
            });

            _logger.LogDebug($"Requesting client for name: {name}, at 'https://{name}.client:443'");

            GrpcChannel channel = GrpcChannel.ForAddress($"https://{name}.client:443", new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
            
            return new Estimates.EstimatesClient(channel);
        }

        public async Task<List<Estimates.EstimatesClient>> GetClients(string token)
        {

            _logger.LogDebug($"Getting kubernetes clients");

            var list = await _kubernetes.CoreV1.ListNamespacedServiceAsync(_namespace, labelSelector: _labelStr);
            _logger.LogDebug($"Received clients: {list}");
            List<Estimates.EstimatesClient> Clients = new List<Estimates.EstimatesClient>();
            foreach (var client in list.Items)
            {
                _logger.LogDebug($"kubernetes client: {client.Metadata.Name}");
                Clients.Add(GetClientByName(client.Metadata.Name, token));
            }
            return Clients;
        }
    }
}
