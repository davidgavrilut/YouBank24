using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YouBank24.Models;
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

    #region API CALLS
    public IActionResult GetTransactions() {
        _claimsIdentity = (ClaimsIdentity)User.Identity;
        _claim = _claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        string loggedUserAccountId = _unitOfWork.Account.GetFirstOrDefault(a => a.ApplicationUserId == _claim.Value).AccountId;
        IEnumerable<Transaction> transactionsSent = _unitOfWork.Transaction.GetAll(t => t.AccountId == loggedUserAccountId);
        List<object> transactionsSentList = new List<object>();
        foreach (Transaction transaction in transactionsSent) {
            if (transaction.TransactionStatus == StaticDetails.StatusSuccess) {
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == transaction.ReceiverUserId);
                transactionsSentList.Add(new { name = user.FirstName + " " + user.LastName, email = user.Email, amount = transaction.Amount, timestamp = transaction.TransactionTimestamp });
            }
        }
        string sendingUserAccountId = null;
        IEnumerable<Transaction> transactionsReceived = _unitOfWork.Transaction.GetAll(t => t.ReceiverUserId == _claim.Value);
        List<object> transactionsReceivedList = new List<object>();
        foreach (Transaction transaction in transactionsReceived) {
            if (transaction.TransactionStatus == StaticDetails.StatusSuccess) {
                sendingUserAccountId = _unitOfWork.Account.GetFirstOrDefault(a => a.AccountId == transaction.AccountId).ApplicationUserId;
                var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == sendingUserAccountId);
                transactionsReceivedList.Add(new { name = user.FirstName + " " + user.LastName, email = user.Email, amount = transaction.Amount, timeStamp = transaction.TransactionTimestamp });
            }
        }
        return Json(new {transactionsSent = transactionsSentList, transactionsReceived = transactionsReceivedList });
    }
    #endregion
}
