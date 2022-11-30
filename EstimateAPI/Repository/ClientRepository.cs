﻿using Grpc.Net.Client;
using InternalAPI;
using k8s;
using EstimateAPI.Configuration;
using Microsoft.Extensions.Options;

namespace EstimateAPI.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientDiscoveryOptions _options;
        private readonly IKubernetes _kubernetes;
        private readonly string _labelStr;
        private CancellationTokenSource _cts;
        private string _namespace;
        public Dictionary<string, Estimates.EstimatesClient> Clients { get; private set; }

        ClientRepository(IOptions<ClientDiscoveryOptions> options) : this(options.Value.Namespace, options) {}

        ClientRepository(string Namespace, IOptions<ClientDiscoveryOptions> options)
        {
            this._namespace = Namespace;
            _options = options.Value;
            _cts = new CancellationTokenSource();
            new Thread(async () => 
            {
                await this.Run(_cts.Token);
            }).Start();
            this.Clients = new Dictionary<string, Estimates.EstimatesClient>();

            // Get the Kubernetes Object
            var config = KubernetesClientConfiguration.InClusterConfig();
            this._kubernetes = new Kubernetes(config);

            // Construct the label filter
            List<string> labelStrs = new List<string>();
            foreach(var option in _options.Labels)
            {
                labelStrs.Add($"{option.Key}={option.Value}");
            }
            _labelStr = string.Join(",", labelStrs.ToArray());
        }

        ~ClientRepository()
        {
            this._cts.Cancel();
        }

        // Summary: Updates the clients every 10 seconds
        public async Task Run(CancellationToken token)
        {
            // Run until cancelled
            while(token.IsCancellationRequested)
            {
                await this.RefreshClients();
                System.Threading.Thread.Sleep(10000); //  Check every 10 seconds
            }
        }
        public async Task RefreshClients()
        {
            var list = await _kubernetes.CoreV1.ListNamespacedServiceAsync(this._namespace, labelSelector: _labelStr);
            Dictionary<string, Estimates.EstimatesClient> Clients = new Dictionary<string, Estimates.EstimatesClient>();
            foreach(var client in list)
            {
                GrpcChannel channel = GrpcChannel.ForAddress($"{client.Metadata.Name}.client");
                Clients.Add(client.Metadata.Name, new Estimates.EstimatesClient(channel));
            }
            this.Clients = Clients;
        }
    }
}