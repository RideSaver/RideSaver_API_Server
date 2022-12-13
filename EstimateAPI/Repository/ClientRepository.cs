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
        private CancellationTokenSource _cts;

        private readonly IKubernetes _kubernetes;
        private readonly string _labelStr;
        private string _namespace;
        public Dictionary<string, Estimates.EstimatesClient> Clients { get; private set; }

        public ClientRepository(IOptions<ClientDiscoveryOptions> options) : this(options.Value.Namespace, options) { }

        public ClientRepository(string Namespace, IOptions<ClientDiscoveryOptions> options)
        {
            _namespace = Namespace;
            _options = options.Value;

            _cts = new CancellationTokenSource();
            new Thread(async () =>
            {
                await Run(_cts.Token);

            }).Start();

            Clients = new Dictionary<string, Estimates.EstimatesClient>();

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

        ~ClientRepository()
        {
            _cts.Cancel();
        }

        // Summary: Updates the clients every 10 seconds
        public async Task Run(CancellationToken token)
        {
            while (token.IsCancellationRequested)  // Run until cancelled
            {
                await RefreshClients();
                Thread.Sleep(10000); //  Check every 10 seconds
            }
        }
        public async Task RefreshClients()
        {
            var list = await _kubernetes.CoreV1.ListNamespacedServiceAsync(_namespace, labelSelector: _labelStr);
            Dictionary<string, Estimates.EstimatesClient> Clients = new();
            foreach (var client in list)
            {
                GrpcChannel channel = GrpcChannel.ForAddress($"https://{client.Metadata.Name}.client");
                Clients.Add(client.Metadata.Name, new Estimates.EstimatesClient(channel));
            }
            this.Clients = Clients;
        }
    }
}
