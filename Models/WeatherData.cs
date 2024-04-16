namespace WeatherConsume.Models
{
    public class WeatherData
    {
        public string? Kind { get; set; }
        public City? City { get; set; }
        public Date? Date { get; set; }
        public string? Description { get; set; }
        public Precipitation? Precipitation { get; set; }
        public Pressure? Pressure { get; set; }
        public Temperature? Temperature { get; set; }
        public Cloudiness? Cloudiness { get; set; }
    }
}
