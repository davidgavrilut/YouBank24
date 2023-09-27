using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YouBank24.Models;
using YouBank24.Repository.IRepository;
using YouBank24.Utils;

namespace YouBank24.Controllers; 
public class SendMoneyController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private ClaimsIdentity _claimsIdentity;
    private Claim _claim;


    public SendMoneyController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IEmailSender emailSender) {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

    [Route("Send")]
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public IActionResult Index() {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public IActionResult SendMoney(Transaction transaction, string email) {
        if (!ModelState.IsValid) {
            _claimsIdentity = (ClaimsIdentity)User.Identity;
            _claim = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            transaction.TransactionId = Guid.NewGuid().ToString();
            transaction.TransactionType = StaticDetails.TransactionTypeInstantMoney;
            transaction.TransactionTimestamp = DateTime.Now;
            var receiverUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(r => r.Email == email);
            transaction.ReceiverUserId = receiverUser.Id;
            transaction.AccountId = _unitOfWork.Account.GetFirstOrDefault(a => a.ApplicationUserId == _claim.Value).AccountId;
            var senderUserId = _unitOfWork.Account.GetFirstOrDefault(si => si.AccountId == transaction.AccountId).ApplicationUserId;
            var senderUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(s => s.Id == senderUserId);
            _unitOfWork.Account.Update(_claim.Value, -transaction.Amount);
            _unitOfWork.Account.Update(transaction.ReceiverUserId, transaction.Amount);
            transaction.TransactionStatus = StaticDetails.StatusSuccess;
            _unitOfWork.Transaction.Add(transaction);
            _unitOfWork.Save();
            _emailSender.SendEmailAsync(senderUser.Email, EmailMessages.SenderMessageSubject, EmailMessages.SenderMessageBody(senderUser.FirstName, senderUser.LastName, receiverUser.FirstName, receiverUser.LastName, transaction.Amount, transaction.Note));
            _emailSender.SendEmailAsync(receiverUser.Email, EmailMessages.ReceiverMessageSubject, EmailMessages.ReceiverMessageBody(senderUser.FirstName, senderUser.LastName, receiverUser.FirstName, receiverUser.LastName, transaction.Amount, transaction.Note));
            if (transaction.Note?.Length > 0) {
                TempData["success"] = "$" + transaction.Amount + " was successfully sent to " + receiverUser.FirstName + " " + receiverUser.LastName + " with the note \"" + transaction.Note + "\"";
            } else {
                TempData["success"] = "$" + transaction.Amount + " was successfully sent to " + receiverUser.FirstName + " " + receiverUser.LastName + ".";
            }
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
