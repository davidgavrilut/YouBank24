using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YouBank24.Models;
using YouBank24.Models.ViewModels;
using YouBank24.Repository.IRepository;
using YouBank24.Services;

namespace YouBank24.Controllers;
public class TransactionListController : Controller {
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private ClaimsIdentity? _claimsIdentity;
    private Claim? _claim;

    public TransactionListController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [AutoValidateAntiforgeryToken]
    [Authorize]
    public IActionResult Index() {
        return View();
    }

    [AutoValidateAntiforgeryToken]
    [Authorize]
    public IActionResult TransactionDetails(string id) {
        var transaction = _unitOfWork.Transaction.GetTransactionById(id);
        var receiverUser = _unitOfWork.ApplicationUser.GetUserById(transaction.ReceiverUserId);
        var senderUserId = _unitOfWork.Account.GetAccountById(transaction.AccountId).ApplicationUserId;
        var senderUser = _unitOfWork.ApplicationUser.GetUserById(senderUserId);

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
    public IActionResult GetTransactions()
    {
        _claimsIdentity = (ClaimsIdentity?)User.Identity;
        _claim = _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

        if (_claim == null)
        {
            throw new NullReferenceException(nameof(_claim));
        }

        List<dynamic> transactionsSentList = GenerateTransactionsList(true);
        List<dynamic> transactionsReceivedList = GenerateTransactionsList(false);

        transactionsSentList = transactionsSentList.OrderByDescending(t => t.timestamp).ToList();
        transactionsReceivedList = transactionsReceivedList.OrderByDescending(t => t.timestamp).ToList();

        return Json(new { transactionsSent = transactionsSentList, transactionsReceived = transactionsReceivedList });
    }

    private List<dynamic> GenerateTransactionsList(bool isSender)
    {
        string? loggedUserAccountId = isSender ? _unitOfWork.Account.GetAccountByUserId(_claim.Value).AccountId : null;
        IEnumerable<Transaction> transactions = isSender ? _unitOfWork.Transaction.GetAllTransactionsByAccountId(loggedUserAccountId) : _unitOfWork.Transaction.GetAllTransactionsByReceiverUserId(_claim.Value);
        List<dynamic> transactionsList = new List<dynamic>();
        foreach (Transaction transaction in transactions)
        {
            if (transaction.TransactionStatus == StaticDetails.StatusSuccess)
            {
                ApplicationUser user;
                if (isSender)
                {
                user = _unitOfWork.ApplicationUser.GetUserById(transaction.ReceiverUserId);
                } 
                else
                {
                    string sendingUserAccountId = _unitOfWork.Account.GetAccountById(transaction.AccountId).ApplicationUserId;
                    user = _unitOfWork.ApplicationUser.GetUserById(sendingUserAccountId);
                }
                transactionsList.Add(new { id = transaction.TransactionId, name = user.FirstName + " " + user.LastName, email = user.Email, amount = transaction.Amount, timestamp = transaction.TransactionTimestamp });
            }
        }

        return transactionsList;
    }
    #endregion
}
