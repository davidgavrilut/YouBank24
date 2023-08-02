using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YouBank24.Models;
using YouBank24.Models.ViewModels;
using YouBank24.Repository.IRepository;
using YouBank24.Utils;

namespace YouBank24.Controllers;
public class TransactionListController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private ClaimsIdentity _claimsIdentity;
    private Claim _claim;

    public TransactionListController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index() {
        return View();
    }

    [AutoValidateAntiforgeryToken]
    [Authorize] 
    public IActionResult TransactionDetails(string id) {
        var transaction = _unitOfWork.Transaction.GetFirstOrDefault(t => t.TransactionId == id);
        var receiverUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(r => r.Id == transaction.ReceiverUserId);
        var senderUserId = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountId == transaction.AccountId).ApplicationUserId;
        var senderUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(s => s.Id == senderUserId);

        var transactionDetailsViewModel = new TransactionDetailsViewModel {
            SenderFirstName = senderUser.FirstName,
            SenderLastName = senderUser.LastName,
            SenderEmail = senderUser.Email,
            ReceiverFirstName = receiverUser.FirstName,
            ReceiverLastName = receiverUser.LastName,
            ReceiverEmail = receiverUser.Email,
            Amount = transaction.Amount,
            Timestamp = transaction.TransactionTimestamp.ToString("MM/dd/yyyy HH:mm:ss"),
            Note = transaction.Note ?? ""
        };

        return View("TransactionDetails", transactionDetailsViewModel);
    }

    #region API CALLS
    [HttpGet("GetTransactions")]
    public IActionResult GetTransactions() {
        _claimsIdentity = (ClaimsIdentity)User.Identity;
        _claim = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        string loggedUserAccountId = _unitOfWork.Account.GetFirstOrDefault(a => a.ApplicationUserId == _claim.Value).AccountId;
        IEnumerable<Transaction> transactionsSent = _unitOfWork.Transaction.GetAll(t => t.AccountId == loggedUserAccountId);
        List<dynamic> transactionsSentList = new List<dynamic>();
        foreach (Transaction transaction in transactionsSent) {
            if (transaction.TransactionStatus == StaticDetails.StatusSuccess) {
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == transaction.ReceiverUserId);
                transactionsSentList.Add(new { id = transaction.TransactionId, name = user.FirstName + " " + user.LastName, email = user.Email, amount = transaction.Amount, timestamp = transaction.TransactionTimestamp.ToString("MM/dd/yyyy HH:mm:ss") });
            }
        }
        string sendingUserAccountId = null;
        IEnumerable<Transaction> transactionsReceived = _unitOfWork.Transaction.GetAll(t => t.ReceiverUserId == _claim.Value);
        List<dynamic> transactionsReceivedList = new List<dynamic>();
        foreach (Transaction transaction in transactionsReceived) {
            if (transaction.TransactionStatus == StaticDetails.StatusSuccess) {
                sendingUserAccountId = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountId == transaction.AccountId).ApplicationUserId;
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == sendingUserAccountId);
                transactionsReceivedList.Add(new { id = transaction.TransactionId, name = user.FirstName + " " + user.LastName, email = user.Email, amount = transaction.Amount, timestamp = transaction.TransactionTimestamp.ToString("MM/dd/yyyy HH:mm:ss") });
            }
        }

        transactionsSentList = transactionsSentList.OrderByDescending(t => t.timestamp).ToList();
        transactionsReceivedList = transactionsReceivedList.OrderByDescending(t => t.timestamp).ToList();

        return Json(new {transactionsSent = transactionsSentList, transactionsReceived = transactionsReceivedList });
    }
    #endregion
}
