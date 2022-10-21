namespace RideSaverAPI.Models
{
    public class Receipt
    {
        public Request? request { get; set; }
        public double subtotal { get; set; }
        public double total_charged { get; set; }
        public double total_owed { get; set; }
        public double total_fare { get; set; }
        public string? duration { get; set; }
        public string? currency_code { get; set; }
        public string? distance { get; set; }
        public string? distance_label { get; set; }



    }
}
