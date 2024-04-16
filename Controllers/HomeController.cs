using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using WeatherConsume.Models;

namespace WeatherConsume.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index(double latitude = 53.893009, double longitude = 27.567444, string req = "current")
        {
            List<WeatherData>? weatherDataList = new List<WeatherData>();
            string latitudeS = latitude.ToString(CultureInfo.InvariantCulture);
            string longitudeS = longitude.ToString(CultureInfo.InvariantCulture);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://api.gismeteo.net/v3/weather/");
                httpClient.DefaultRequestHeaders.Add("X-Gismeteo-Token", "55431274-2040-4600-ab60-2ea85928597a"); // —юда токен
                using (var response = await httpClient.GetAsync($"{req}/?latitude={latitudeS}&longitude={longitudeS}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        if (req == "current")
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            JSONDeserializeModel<WeatherData> deserializedObj = JsonConvert.DeserializeObject<JSONDeserializeModel<WeatherData>>(apiResponse);
                            weatherDataList.Add(deserializedObj.data);
                            
                        }
                        else
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            JSONDeserializeModel<List<WeatherData>> deserializedList = JsonConvert.DeserializeObject<JSONDeserializeModel<List<WeatherData>>>(apiResponse);
                            foreach (var wether in deserializedList.data)
                            {
                                weatherDataList.Add(wether);
                            }
                        }
                    }
                }
            }
            return View(weatherDataList);
        }

        [HttpPost]
        public IActionResult Export(List<WeatherData> weatherDataList, string req = "current")
        {
            
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Brands");

                worksheet.Cell("A1").Value = "Time";
                worksheet.Cell("B1").Value = "Air Temp";
                worksheet.Cell("C1").Value = "Comfort Temp";
                worksheet.Cell("D1").Value = "Water Temp";
                worksheet.Cell("E1").Value = "Pressure";
                worksheet.Cell("F1").Value = "Cloudiness";
                worksheet.Cell("G1").Value = "Description";
                worksheet.Row(1).Style.Font.Bold = true;
                //нумераци€ строк/столбцов начинаетс€ с индекса 1 (не 0)
                int cellCounter = 2;
                foreach(WeatherData weatherData in weatherDataList)
                {
                    if (req == "current" || req == null)
                    {
                        AddToWorksheet(worksheet, cellCounter, weatherData);
                        cellCounter++;
                    }
                    else
                    {
                        if (DateTime.Now.AddHours(-2) <= weatherData.Date.Local)
                        {
                            if (req == "forecast/h1")
                            {
                                if (weatherData.Date.Local <= DateTime.Now.AddDays(1))
                                {

                                }
                            }
                            else if (req == "forecast/h6")
                            {
                                if (weatherData.Date.Local <= DateTime.Now.AddDays(7))
                                {
                                    AddToWorksheet(worksheet, cellCounter, weatherData);
                                    cellCounter++;
                                }
                            }
                            else
                            {
                                AddToWorksheet(worksheet, cellCounter, weatherData);
                            }
                        }
                    }
                        
                }


                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"weather_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
        public void AddToWorksheet(IXLWorksheet worksheet, int rowNumber, WeatherData data)
        {
            worksheet.Cell(rowNumber, 1).Value = data.Date.Local;
            worksheet.Cell(rowNumber, 2).Value = data.Temperature?.Air.c;
            worksheet.Cell(rowNumber, 3).Value = data.Temperature?.Comfort.c;
            worksheet.Cell(rowNumber, 4).Value = data.Temperature?.Water.c;
            worksheet.Cell(rowNumber, 5).Value = data.Pressure?.mm_hg_atm;
            worksheet.Cell(rowNumber, 6).Value = data.Cloudiness?.Percent;
            worksheet.Cell(rowNumber, 7).Value = data.Description;
        }
    }
}
