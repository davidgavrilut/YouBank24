using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Util;
using NuGet.Protocol;
using System.Security.Claims;
using System.Text.Json;
using YouBank24.Models.ViewModels;
using YouBank24.Repository.IRepository;
using YouBank24.Services;
using YouBank24.Services.IServices;

namespace YouBank24.Controllers
{
    public class LoanController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailMessage _emailMessage;
        private ClaimsIdentity? _claimsIdentity;
        private Claim? _claim;
        public LoanController(HttpClient httpClient, IEmailSender emailSender, IUnitOfWork unitOfWork, IEmailMessage emailMessage)
        {
            _httpClient = httpClient;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _emailMessage = emailMessage;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<InterestRate> GetInterest(string country)
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
            var filteredRatesData = interestRateData?.central_bank_rates
                .Where(r => r.country == country)
                .FirstOrDefault();
            if (filteredRatesData != null)
            {
                return new InterestRate
                {
                    central_bank = filteredRatesData.central_bank,
                    country = filteredRatesData.country,
                    rate_pct = filteredRatesData.rate_pct,
                    last_updated = filteredRatesData.last_updated
                };
            }
            throw new BadHttpRequestException("No data was found for the specified country");
        }

        [HttpGet]
        public IActionResult Simulate(int amount, int period, string country)
        {
            var result = GetInterest(country).Result;
            var monthlyInterest = result.rate_pct / 12 / 100;

            var monthlyPayment = amount * (monthlyInterest * Math.Pow((1 + monthlyInterest), period)) / (Math.Pow((1 + monthlyInterest), period) - 1);
            var totalPayableAmount = monthlyPayment * period;

            return Json(new
            {
                monthlyPayment,
                totalPayableAmount,
                country,
                centralBank = result.central_bank,
                interest = result.rate_pct,
                lastUpdated = result.last_updated,
            });
        }

        [HttpPost]
        public IActionResult SendSimulationEmail(string country, int amount, int period, double monthlyPayment, double interest, double totalPayableAmount, string centralBank, string lastUpdated)
        {
            _claimsIdentity = (ClaimsIdentity?)User.Identity;
            _claim = _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            
            if (_claim == null) {
                throw new NullReferenceException(nameof(_claim));
            }

            var userEmail = _unitOfWork.ApplicationUser.GetUserById(_claim.Value).Email;
            _emailSender.SendEmailAsync(
                userEmail,
                _emailMessage.SimulationEmailSubject,
                _emailMessage.SimulationEmailBody(country, amount, period, monthlyPayment, interest, totalPayableAmount, centralBank, lastUpdated)
            );
            return Ok();
        }
    }
}


