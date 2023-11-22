using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YouBank24.Models;
using YouBank24.Repository.IRepository;
using YouBank24.Services;
using YouBank24.Services.IServices;

namespace YouBank24.Controllers; 
public class SendMoneyController : Controller {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly ITransactionService _transactionService;
    private ClaimsIdentity? _claimsIdentity;
    private Claim? _claim;


    public SendMoneyController(IUnitOfWork unitOfWork, IEmailSender emailSender, ITransactionService transactionService) {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _transactionService = transactionService;
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
            _claimsIdentity = (ClaimsIdentity?)User.Identity;
            _claim = _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (_claim == null)
            {
                throw new NullReferenceException(nameof(_claim));
            }

            var newTransaction = _transactionService.CreateTransaction(transaction, email, _claim);
            
            _unitOfWork.Account.UpdateBalance(_claim.Value, -newTransaction.Transaction.Amount);
            _unitOfWork.Account.UpdateBalance(newTransaction.Transaction.ReceiverUserId, newTransaction.Transaction.Amount);
            
            newTransaction.Transaction.TransactionStatus = StaticDetails.StatusSuccess;
            _unitOfWork.Transaction.Add(transaction);
            _unitOfWork.Save();

            _emailSender.SendEmailAsync(newTransaction.SenderUser.Email, EmailMessages.SenderMessageSubject, EmailMessages.SenderMessageBody(newTransaction.SenderUser.FirstName, newTransaction.SenderUser.LastName, newTransaction.ReceiverUser.FirstName, newTransaction.ReceiverUser.LastName, newTransaction.Transaction.Amount, newTransaction.Transaction.Note));
            _emailSender.SendEmailAsync(newTransaction.ReceiverUser.Email, EmailMessages.ReceiverMessageSubject, EmailMessages.ReceiverMessageBody(newTransaction.SenderUser.FirstName, newTransaction.SenderUser.LastName, newTransaction.ReceiverUser.FirstName, newTransaction.ReceiverUser.LastName, newTransaction.Transaction.Amount, newTransaction.Transaction.Note));
            
            if (newTransaction.Transaction.Note?.Length > 0) {
                TempData["success"] = "$" + newTransaction.Transaction.Amount + " was successfully sent to " + newTransaction.ReceiverUser.FirstName + " " + newTransaction.ReceiverUser.LastName + " with the note \"" + newTransaction.Transaction.Note + "\"";
            } else {
                TempData["success"] = "$" + newTransaction.Transaction.Amount + " was successfully sent to " + newTransaction.ReceiverUser.FirstName + " " + newTransaction.ReceiverUser.LastName + ".";
            }
            return RedirectToAction("Index", "Home");
        } else {
            return RedirectToAction("Index", "SendMoney");
        }
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetSendMoneyData() {
        _claimsIdentity = (ClaimsIdentity?)User.Identity;
        _claim = _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

        if (_claim == null)
        {
            throw new NullReferenceException(nameof(_claim));
        }

        IEnumerable<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAllUsersExceptCurrentUser(_claim.Value);
        List<object> sendMoneyData = new List<object>();
        foreach(var user in users) {
            sendMoneyData.Add( new { firstName = user.FirstName, lastName = user.LastName, email = user.Email });
        }
        return Json(sendMoneyData);
    }

    public IActionResult GetUserBalance() {
        _claimsIdentity = (ClaimsIdentity?)User.Identity;
        _claim = _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

        if (_claim == null)
        {
            throw new NullReferenceException(nameof(_claim));
        }

        var balance = _unitOfWork.Account.GetAccountByUserId(_claim.Value).Balance;
        return Json(balance);
    }
    #endregion
}
