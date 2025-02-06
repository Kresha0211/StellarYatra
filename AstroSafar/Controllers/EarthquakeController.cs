using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using AstroSafar.Models;
using System.Diagnostics;
using System.Numerics;

namespace AstroSafar.Controllers
{
    public class EarthquakeController : Controller
    {
        private readonly HttpClient _httpClient;
        public EarthquakeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            //string apiUrl = "https://planets-info-by-newbapi.p.rapidapi.com/api/v1/planets/"; // Replace with the correct endpoint
            //string apiHost = "planets-info-by-newbapi.p.rapidapi.com";
            //string apiKey = "bfc67953b0msh910ffbc75a90df9p16c8b7jsn8f69a655427b"; // Replace with your actual API key

            //var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            //request.Headers.Add("x-rapidapi-key", apiKey);
            //request.Headers.Add("x-rapidapi-host", apiHost);

            //var response = await _httpClient.SendAsync(request);
            //if (!response.IsSuccessStatusCode)
            //{
            //    return View("Error");
            //}

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.nasa.gov/planetary/earth/imagery?lon=100.75&lat=1.5&date=2024-02-01&api_key=bzVxiHUULEp9m7NIGpaT75oo7k1JgnbmugkEeWsM"),
                Headers =
                {
                    //{ "x-rapidapi-key", "bzVxiHUULEp9m7NIGpaT75oo7k1JgnbmugkEeWsM" },
                    //{ "x-rapidapi-host", "everyearthquake.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }

            //var jsonString = await response.Content.ReadAsStringAsync();
            //var apiResponse = JsonConvert.DeserializeObject<EarthquakeApiResponse>(jsonString);

            //if (apiResponse?.Earthquakes == null)
            //{
            //    return View(new List<Earthquake>()); // Return empty list if data is null
            //}
            //
            //return View(apiResponse.Earthquakes);
            return Ok();
        }


    }
    //return View();
}



