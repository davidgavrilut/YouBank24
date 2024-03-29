﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YouBank24.Models;
using YouBank24.Models.ViewModels;
using YouBank24.Repository.IRepository;
using YouBank24.Services;
using YouBank24.Services.IServices;

namespace YouBank24.Controllers;
[Authorize]
public class SendMoneyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly ITransactionService _transactionService;
    private readonly IEmailMessage _emailMessage;
    private ClaimsIdentity? _claimsIdentity;
    private Claim? _claim;


    public SendMoneyController(IUnitOfWork unitOfWork, IEmailSender emailSender, ITransactionService transactionService, IEmailMessage emailMessage)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _transactionService = transactionService;
        _emailMessage = emailMessage;
    }

    [Route("Send")]
    [AutoValidateAntiforgeryToken]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public IActionResult SendMoney(Transaction transaction, string email)
    {
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

        _emailSender.SendEmailAsync(newTransaction.SenderUser.Email, _emailMessage.SenderMessageSubject, _emailMessage.GenerateSenderEmailBody(newTransaction));
        _emailSender.SendEmailAsync(newTransaction.ReceiverUser.Email, _emailMessage.ReceiverMessageSubject, _emailMessage.GenerateReceiverEmailBody(newTransaction));

        if (newTransaction.Transaction.Note?.Length > 0)
        {
            TempData["success"] = $"${newTransaction.Transaction.Amount} was successfully sent to {newTransaction.ReceiverUser.FirstName} {newTransaction.ReceiverUser.LastName} with the note \"{newTransaction.Transaction.Note}\"";
        }
        else
        {
            TempData["success"] = $"${newTransaction.Transaction.Amount} was successfully sent to {newTransaction.ReceiverUser.FirstName} {newTransaction.ReceiverUser.LastName}";
        }
        return RedirectToAction("Index", "Home");
    }

    #region API CALLS
    [HttpGet]
    [AutoValidateAntiforgeryToken]
    public IActionResult GetSendMoneyData()
    {
        _claimsIdentity = (ClaimsIdentity?)User.Identity;
        _claim = _claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

        if (_claim == null)
        {
            throw new NullReferenceException(nameof(_claim));
        }

        IEnumerable<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAllUsersExceptCurrentUser(_claim.Value);
        List<SendMoneyUsersData> sendMoneyData = new List<SendMoneyUsersData>();
        foreach (var user in users)
        {
            sendMoneyData.Add(new SendMoneyUsersData() { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email });
        }
        return Json(sendMoneyData);
    }

    [HttpGet]
    [AutoValidateAntiforgeryToken]
    public IActionResult GetUserBalance()
    {
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
