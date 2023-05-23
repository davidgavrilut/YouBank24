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
            transaction.TransactionTimestamp = DateTime.Now;
            transaction.TransactionStatus = StaticDetails.StatusPending;
            transaction.ReceiverUserId = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Email == email).Id;
            transaction.AccountId = _unitOfWork.Account.GetFirstOrDefault(a => a.ApplicationUserId == _claim.Value).AccountId;
            _unitOfWork.Transaction.Add(transaction);
            _unitOfWork.Account.Update(_claim.Value, -transaction.Amount);
            _unitOfWork.Account.Update(transaction.ReceiverUserId, transaction.Amount);
            _unitOfWork.Save();
            return RedirectToAction("Index", "Home");
        } else
            return RedirectToAction("Index", "SendMoney");
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetSendMoneyData() {
        _claimsIdentity = (ClaimsIdentity)User.Identity;
        _claim = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        IEnumerable<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAll(x => x.Id != _claim.Value);
        List<object> usersBriefData = new List<object>();
        foreach(var user in users) {
            usersBriefData.Add( new { firstName = user.FirstName, lastName = user.LastName, email = user.Email });
        }
        return Json(usersBriefData);
    }   
    #endregion
}
