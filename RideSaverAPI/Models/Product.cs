namespace RideSaverAPI.Models
{
    public class Product
    {
        public bool upfront_fare_enabled { get; set; }
        public int capacity { get; set; }
        public PriceDetails? priceDetails { get; set; }
        public bool cash_enabled { get; set; }
        public bool shared_ride { get; set; }
        public string? short_description { get; set; }
        public string? display_name { get; set; }
        public string? product_group { get; set; }


    }
}
