using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using YouBank24.Models;
using YouBank24.Models.ViewModels;
using YouBank24.Repository.IRepository;

namespace YouBank24.Controllers; 
public class HomeController : Controller {
    private readonly IUnitOfWork _unitOfWork;
    private ClaimsIdentity? _claimsIdentity;
    private Claim? _claim;


    public HomeController(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index() {
        _claimsIdentity = (ClaimsIdentity?)User.Identity;
        _claim = _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
        if (_claim != null) {
            return RedirectToAction(nameof(MainAccount));
        } else {
            return View();
        }
    }

    [Route("Main")]
    public IActionResult MainAccount() {
        _claimsIdentity = (ClaimsIdentity?)User.Identity;
        _claim = _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

        if (_claim != null) {
            AccountViewModel accountViewModel = new() {
                Account = _unitOfWork.Account.GetAccountByUserId(_claim.Value),
                ApplicationUser = _unitOfWork.ApplicationUser.GetUserById(_claim.Value)
            };
            return View(accountViewModel);
        } else {
            return NotFound();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    public IActionResult Privacy() {
        return View();
    }

    #region API CALLS

    [HttpGet]
    public IActionResult GetAccountCardNumberBalance() {
        _claimsIdentity = (ClaimsIdentity?)User.Identity;
        _claim = _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

        if (_claim  == null)
        {
            throw new NullReferenceException(nameof(_claim));
        }

        var account = _unitOfWork.Account.GetAccountByUserId(_claim.Value);
        return Json(new { cardNumber = account.CardNumber, balance = account.Balance });
    }

    #endregion
}