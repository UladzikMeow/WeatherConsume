namespace WeatherConsume.Models
{
    public class Cloudiness
    {
        public int Percent {  get; set; }
        public int Scale_3 { get; set; } // тип облочности 0 - Ясно, 1 - Малооблачно, 2 - Облачно, 3 - Пасмурно, 101 - Переменная облачность

    }
}
