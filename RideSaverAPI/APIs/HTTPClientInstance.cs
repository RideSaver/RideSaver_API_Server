namespace RideSaverAPI.APIs
{
    public static class HTTPClientInstance // Static class, gets initalized once to avoid opening multiple ports.
    {
        public static HttpClient? APIClientInstance { get; set; } // Static instance of HTTP Client -> Used to open ONE TCP port to handle all the communications to other APIs.
        public static void InitializeClient() // Initalizes the APIClientInstance when invoked in Program.cs on application startup.
        {
            APIClientInstance = new HttpClient();
            APIClientInstance.DefaultRequestHeaders.Accept.Clear(); // Clears the headers.
            APIClientInstance.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")); // Allows the headers to accept JSON objects.
        }
    }
}
