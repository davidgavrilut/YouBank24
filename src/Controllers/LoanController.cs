using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Util;
using NuGet.Protocol;
using System.Text.Json;
using YouBank24.Data;

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

        public async Task<double> GetInterest(string country)
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

            using var stream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var interestRateData = await JsonSerializer.DeserializeAsync<InterestRateData>(stream, options);
            var filteredRates = interestRateData?.central_bank_rates
                .Where(r => r.country == country)
                .Select(s => s.rate_pct).FirstOrDefault();
            if (filteredRates != null)
            {
                return (double)filteredRates;
            }
            throw new BadHttpRequestException("No data was found for the specified country");
        }

        [HttpGet]
        public async Task<IActionResult> Simulate(int amount, int period, string country)
        {
            var interest = await GetInterest(country);
            var monthlyInterest = interest / 12 / 100;


            var monthlyPayment = amount * (monthlyInterest * Math.Pow((1 + monthlyInterest), period)) / (Math.Pow((1 + monthlyInterest), period) - 1);
                var totalPayableAmount = monthlyPayment * period;

                return Json(new { monthlyPayment, totalPayableAmount, interest });
            }
    }
}


