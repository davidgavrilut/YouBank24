using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YouBank24.Controllers
{
    public class LoanController : Controller
    {
        private HttpClient _httpClient;
        public LoanController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetInterest(string country)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://interest-rate-by-api-ninjas.p.rapidapi.com/v1/interestrate/"),
                Headers =
                {
                    { "X-RapidAPI-Key", "f339be2775mshd41d71973094cf5p1908eajsn24a0b466000a" },
                    { "X-RapidAPI-Host", "interest-rate-by-api-ninjas.p.rapidapi.com" },
                },
            };
            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return Ok(body);
        }

        public async Task<IActionResult> Simulate(int amount, int period, string country)
        {
            var interest = await GetInterest(country);
            // calculate monthly
            // calculate total payable amount
            // return monthly, total amount, interest
            return Json(new { });
        }
    }
}
