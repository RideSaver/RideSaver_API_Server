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

        private readonly IKubernetes _kubernetes;
        private readonly string _labelStr;
        private string _namespace;

        public ClientRepository(IOptions<ClientDiscoveryOptions> options) : this(options.Value.Namespace, options) { }

        public ClientRepository(string Namespace, IOptions<ClientDiscoveryOptions> options)
        {
            _namespace = Namespace;
            _options = options.Value;

            // Get the Kubernetes Object
            var config = KubernetesClientConfiguration.InClusterConfig();
            _kubernetes = new Kubernetes(config);

            // Construct the label filter
            List<string> labelStrs = new();
            foreach (var option in _options.Labels)
            {
                labelStrs.Add($"{option.Key}={option.Value}");
            }

            _labelStr = string.Join(",", labelStrs.ToArray());
        }

        public Estimates.EstimatesClient GetClientByName(string name)
        {
            GrpcChannel channel = GrpcChannel.ForAddress($"https://{client.Metadata.Name}.client");
            return new Estimates.EstimatesClient(channel);
        }
        public async Task<Estimates.EstimatesClient[]> GetClients()
        {
            var list = await _kubernetes.CoreV1.ListNamespacedServiceAsync(_namespace, labelSelector: _labelStr);
            List<Estimates.EstimatesClient> Clients = new List();
            foreach (var client in list)
            {
                Clients.Add(GetClientByName(client.Metadata.Name));
            }
            this.Clients = Clients;
        }
    }
}
