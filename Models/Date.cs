namespace WeatherConsume.Models
{
    public class Date
    {
        public DateTime UTC {  get; set; }
        public DateTime Local { get; set; }
        public int Unix {  get; set; }
        public int TimeZoneOffset { get; set; } // в минутах
    }
}
