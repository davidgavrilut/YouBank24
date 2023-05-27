using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YouBank24.Models;
using YouBank24.Repository.IRepository;
using YouBank24.Utils;

namespace YouBank24.Controllers; 
public class SendMoneyController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private ClaimsIdentity _claimsIdentity;
    private Claim _claim;


    public SendMoneyController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index() {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SendMoney(Transaction transaction, string email) {
        if (!ModelState.IsValid) {
            _claimsIdentity = (ClaimsIdentity)User.Identity;
            _claim = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            transaction.TransactionId = Guid.NewGuid().ToString();
            transaction.TransactionType = StaticDetails.TransactionTypeInstantMoney;
            transaction.TransactionTimestamp = DateTime.Now;
            var receiverUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Email == email);
            transaction.ReceiverUserId = receiverUser.Id;
            transaction.AccountId = _unitOfWork.Account.GetFirstOrDefault(a => a.ApplicationUserId == _claim.Value).AccountId;
            _unitOfWork.Account.Update(_claim.Value, -transaction.Amount);
            _unitOfWork.Account.Update(transaction.ReceiverUserId, transaction.Amount);
            transaction.TransactionStatus = StaticDetails.StatusSuccess;
            _unitOfWork.Transaction.Add(transaction);
            _unitOfWork.Save();
            TempData["success"] = "$" + transaction.Amount + " was successfully sent to " + receiverUser.FirstName + " " + receiverUser.LastName + ".";
            return RedirectToAction("Index", "Home");
        } else {
            return RedirectToAction("Index", "SendMoney");
        }
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetSendMoneyData() {
        _claimsIdentity = (ClaimsIdentity)User.Identity;
        _claim = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        IEnumerable<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAll(x => x.Id != _claim.Value);
        List<object> sendMoneyData = new List<object>();
        foreach(var user in users) {
            sendMoneyData.Add( new { firstName = user.FirstName, lastName = user.LastName, email = user.Email });
        }
        return Json(sendMoneyData);
    }

    public IActionResult GetUserBalance() {
        _claimsIdentity = (ClaimsIdentity)User.Identity;
        _claim = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        var balance = _unitOfWork.Account.GetFirstOrDefault(a => a.ApplicationUserId == _claim.Value).Balance;
        return Json(balance);
    }
    #endregion
}
