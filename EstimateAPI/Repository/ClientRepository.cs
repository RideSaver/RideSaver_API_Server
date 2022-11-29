using Grpc.Net.Client;
using InternalAPI;
using k8s;

namespace EstimateAPI.Repository
{
    public class EstimateRepository : IEstimateRepository
    {
        private readonly ClientDiscoveryOptions _options;
        private CancellationTokenSource _cts;
        private string _namespace;
        public Dictionary<string, Estimates.EstimatesClient> Clients { get; private set; };

        EstimateRepository(IOptions<PositionOptions> options): this(options.Value.namespace) {}

        EstimateRepository(string namespace, IOptions<PositionOptions> options) {
            this._namespace = namespace;
            _options = options.Value;
            _cancelToken = new CancellationToken()
            _cts = new CancellationTokenSource();
            this.Run(_cts.Token);
        }

        ~EstimateRepository()
        {
            this._cts.Cancel();
        }

        // Summary: Updates the clients every 10 seconds
        public async Task<List<Estimate>> Run(CancellationToken token)
        {
            // Get the Kubernetes Object
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);

            // Construct the label filter
            List<string> labelStrs = "";
            foreach(var option in _options.Labels)
            {
                labelStrs.Add($"{option.Key}={option.Value}");
            }
            string labelStr = string.Join(",", labelStrs.ToArray());

            // Run until cancelled
            while(token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
                var list = client.CoreV1.ListNamespacedService(this.namespace, labelSelector: labelStr);
                Dictionary<string, Estimates.EstimatesClient> Clients;
                foreach(var client in list)
                {
                    Clients.Add(client.meta.name, new Estimates.EstimatesClient($"{client.meta.name}.client"));
                }
                this.Clients = Clients;
                System.Threading.Thread.Sleep(10000); //  Check every 10 seconds
            }
            throw new NotImplementedException();
        }
    }
}
