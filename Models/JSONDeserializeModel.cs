namespace WeatherConsume.Models
{
    public class JSONDeserializeModel<T>
    {
        //public WeatherData data {  get; set; }
        public T data { get; set; }
        public object jsonapi { get; set; }
        public object meta {  get; set; }
    }
}
