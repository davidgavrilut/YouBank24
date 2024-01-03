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
        private readonly IClientInterest _clientInterest;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailMessage _emailMessage;
        private ClaimsIdentity? _claimsIdentity;
        private Claim? _claim;
        public LoanController(IClientInterest clientInterest, IEmailSender emailSender, IUnitOfWork unitOfWork, IEmailMessage emailMessage)
        {
            _clientInterest = clientInterest;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _emailMessage = emailMessage;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Simulate(int amount, int period, string country)
        {
            var result = _clientInterest.GetInterestRateAsync(country).Result;
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

            if (_claim == null)
            {
                throw new NullReferenceException(nameof(_claim));
            }

            var userEmail = _unitOfWork.ApplicationUser.GetUserById(_claim.Value).Email;
            _emailSender.SendEmailAsync(
                userEmail,
                _emailMessage.SimulationEmailSubject,
                _emailMessage.SimulationEmailBody(country, amount, period, monthlyPayment, interest, totalPayableAmount, centralBank, lastUpdated)
            );
            return Json(new { country, amount, period, monthlyPayment, interest, totalPayableAmount, centralBank, lastUpdated});
        }
    }
}


