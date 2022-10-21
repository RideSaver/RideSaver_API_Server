using RideSaver.Server.Models;

namespace RideSaverAPI.Models
{
    public class PriceEstimate
    {
        public string? localized_display_name { get; set; }
        public float distance { get; set; }
        public string? display_name { get; set; }
        public Product? product_id { get; set; }

        public float high_estimate { get; set; }
        public float low_estimate { get; set; }
        public int duration { get; set; }
        public Estimate? estimate { get; set; }
        public double surge_multiplier { get; set; }
        public string? currency_code { get; set; }
    }
}
