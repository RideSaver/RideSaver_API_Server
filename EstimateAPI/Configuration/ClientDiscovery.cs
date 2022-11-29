namespace EstimateAPI.Configuration
{
    public class ClientDiscoveryOptions
    {
        public const string Position = "Clients";

        public string Namespace { get; set; } = "client";
        public Dictionary<string, string> Labels { get; set; } = new Dictionary() {
            {"type", "api-client"}
        };
    }
}
