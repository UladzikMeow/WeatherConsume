namespace WeatherConsume.Models
{
    public class Temperature
    {
        public C Air {  get; set; }
        public C Comfort { get; set; }
        public C Water { get; set; }

    }
    public class C
    {
        public float c { get; set; }
    }
}
