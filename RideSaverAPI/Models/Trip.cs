namespace RideSaverAPI.Models
{
    public class Trip
    {
        public string? alias { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public double eta { get; set; }
    }
}
