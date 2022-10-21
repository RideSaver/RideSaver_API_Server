namespace RideSaverAPI.Models
{
    public class PriceDetails
    {
       public double cost_per_minute { get; set; }
       public string? distance_unit { get; set; }
       public float minmum { get; set; }
       public float cost_per_distance { get; set; }
       public float base_fare { get; set; }
       public float cancellation_fee { get; set; }
       public string? currency_code { get; set; }

       
    }
}
